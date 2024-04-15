using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.SolutionDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly IAssignmentToChallengeRepository _assignmentRepository;
        private readonly ICodingChallengeRepository _codingChallengeRepository;
        private readonly ISolutionFileRepository _fileRepository;
        private IValidationProvider _validationProvider;
        private readonly IUserDetailsProvider _userDetails;

        public SolutionService(IAssignmentToChallengeRepository assignmentRepository,
                               ICodingChallengeRepository codingChallengeRepository,
                               IValidationProvider validationProvider,
                               ISolutionFileRepository fileRepository,
                               IUserDetailsProvider userDetails)
        {
            _assignmentRepository = assignmentRepository;
            _codingChallengeRepository = codingChallengeRepository;
            _validationProvider = validationProvider;
            _fileRepository = fileRepository;
            _userDetails = userDetails;
        }

        public async Task<SolutionResponseDto> SubmitCodeAsync(SolutionRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _codingChallengeRepository.HasAnyAsync(dto.CodingChallengeId))
            {
                throw new NotFoundException("Invalid coding challenge ID.");
            }

            var path = await _fileRepository
                .CreateOrUpdateSolutionFile(dto.CodingChallengeId, _userDetails.GetUserId(), dto.SolutionCode);

            var code = dto.SolutionCode;
            dto.SolutionCode = path;

            var response = await _assignmentRepository.UpdateSolutionPathAsync(_userDetails.GetUserId(), dto);
            response.SolutionCode = code;

            return response;
        }

        public async Task<SolutionResponseDto> GetCodeByCodingChallengeIdAsync(int internId, Guid codingChallengeId)
        {
            if (codingChallengeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(codingChallengeId));
            }

            var solution = await _assignmentRepository.GetSolutionByInternAndChallengeIdsAsync(internId, codingChallengeId);

            if (File.Exists(solution.SolutionCode))
            {
                solution.SolutionCode = File.ReadAllText(solution.SolutionCode);
            }

            return solution;
        }
    }
}
