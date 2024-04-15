using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.CodingChallenge;

namespace Prime.Progreso.Services
{
    public class CodingChallengeService : ICodingChallengeService
    {
        private readonly ICodingChallengeRepository _codingChallengeRepository;
        private readonly IAssignmentToChallengeRepository _assignmentRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private IValidationProvider _validationProvider;
        private readonly IUserDetailsProvider _userDetails;

        public CodingChallengeService(ICodingChallengeRepository codingChallengeRepository,
                                      IValidationProvider validationProvider,
                                      IAssignmentToChallengeRepository assignmentRepository,
                                      ITechnologyRepository technologyRepository,
                                      IUserDetailsProvider userDetailsHelper)
        {
            _codingChallengeRepository = codingChallengeRepository;
            _validationProvider = validationProvider;
            _assignmentRepository = assignmentRepository;
            _technologyRepository = technologyRepository;
            _userDetails = userDetailsHelper;
        }

        public async Task<CodingChallengeResponseDto> CreateAsync(CodingChallengeRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await EnsureTechnologyExistsAsync(dto.TechnologyId);

            return await _codingChallengeRepository.CreateAsync<CodingChallengeRequestDto, CodingChallengeResponseDto>(dto);
        }

        public async Task UpdateAsync(Guid id, CodingChallengeRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await EnsureTechnologyExistsAsync(dto.TechnologyId);

            await _codingChallengeRepository.UpdateAsync<CodingChallengeRequestDto>(id, dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _codingChallengeRepository.DeleteAsync(id);
        }

        public async Task<CodingChallengeResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                var codingChallenge = await _codingChallengeRepository.GetByIdAsync<CodingChallengeResponseDto>(id);

                if(! await IsAccessAllowedAsync(codingChallenge, _userDetails.GetUserId()))
                {
                    throw new UnauthorizedAccessException("You do not have access to this coding challenge.");
                }

                return codingChallenge;
            }

            return await _codingChallengeRepository.GetByIdAsync<CodingChallengeResponseDto>(id);
        }

        public async Task<PaginatedResult<CodingChallengeResponseDto>> GetPageAsync(CodingChallengesPagingInfo pagingInfo)
        {
            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                var codingChallengeIds = await _assignmentRepository.GetAssignedCodingChallengeIdsAsync(_userDetails.GetUserId());

                if(codingChallengeIds.Count == 0)
                {
                    return new PaginatedResult<CodingChallengeResponseDto>(new List<CodingChallengeResponseDto>(), 0, 0, 0);
                }

                pagingInfo.codingChallengeIds = codingChallengeIds;

                return await _codingChallengeRepository.GetPageAsync(pagingInfo);
            }

            return await _codingChallengeRepository.GetPageAsync<CodingChallengeResponseDto>(pagingInfo.Page, pagingInfo.Size);
        }

        private async Task EnsureTechnologyExistsAsync(Guid id)
        {
            if(!await _technologyRepository.HasAnyAsync(id))
            {
                throw new NotFoundException("Technology does not exist!");
            }
        }

        private async Task<bool> IsAccessAllowedAsync(CodingChallengeResponseDto dto, int userId)
        {
            var allowed = true;

            var assignmentExists = await _assignmentRepository.HasAnyAsync(dto.Id, userId);

            if (assignmentExists)
            {
                var assignment = await _assignmentRepository.GetByInternAndChallengeIdsAsync(userId, dto.Id);

                var currentDateTime = DateTime.UtcNow;

                if (currentDateTime < assignment.StartTime
                    || (assignment.EndTime != null && currentDateTime > assignment.EndTime))
                {
                    allowed = false;
                }
            }
            else
            {
                allowed = false;
            }

            return allowed;
        }
    }
}
