using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Services
{
    public class AssignmentToChallengeService : IAssignmentToChallengeService
    {        
        private readonly IAssignmentToChallengeRepository _assignmentRepository;
        private readonly ICodingChallengeRepository _codingChallengeRepository;
        private IValidationProvider _validationProvider;
        private readonly IUserDetailsProvider _userDetails;

        public AssignmentToChallengeService(IAssignmentToChallengeRepository assignmentRepository,
                                            ICodingChallengeRepository codingChallengeRepository,
                                            IValidationProvider validationProvider,
                                            IUserDetailsProvider userDetailsHelper)
        {
            _assignmentRepository = assignmentRepository;
            _codingChallengeRepository = codingChallengeRepository;
            _validationProvider = validationProvider;
            _userDetails = userDetailsHelper;
        }

        public async Task<AssignmentResponseDto> AssignInternAsync(AssignmentRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if(!await _codingChallengeRepository.HasAnyAsync(dto.CodingChallengeId))
            {
                throw new NotFoundException("Invalid coding challenge ID.");
            }

            return await _assignmentRepository.AddOrUpdateAsync(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _assignmentRepository.DeleteAsync(id);
        }

        public async Task<AssignmentResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _assignmentRepository.GetByIdAsync(id, _userDetails.GetUserId());
            }

            return await _assignmentRepository.GetByIdAsync<AssignmentResponseDto>(id);
        }

        public async Task<PaginatedResult<AssignmentResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _assignmentRepository.GetPageAndFilterByUserIdAsync(pageNumber, pageSize, _userDetails.GetUserId());
            }

            return await _assignmentRepository.GetPageAsync<AssignmentResponseDto>(pageNumber, pageSize);
        }

        public async Task UnassignInternAsync(UnassignmentRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _codingChallengeRepository.HasAnyAsync(dto.CodingChallengeId))
            {
                throw new NotFoundException("Invalid coding challenge ID.");
            }

            var existingAssignment = await _assignmentRepository.GetByInternAndChallengeIdsAsync(dto.InternId,
                                                                                                 dto.CodingChallengeId);
            if(existingAssignment.StartTime > dto.EndTime)
            {
                throw new ValidationException("End time cannot be less than the start time!");
            }

            await _assignmentRepository.UnassignInternAsync(dto);
        }
    }
}
