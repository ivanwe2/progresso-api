using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.TestCase;

namespace Prime.Progreso.Services
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ICodingChallengeRepository _codingChallengeRepository;
        private readonly IAssignmentToChallengeRepository _assignmentRepository;
        private readonly IValidationProvider _validationProvider;
        private readonly IUserDetailsProvider _userDetails;

        public TestCaseService(ITestCaseRepository testCaseRepository,
                               IValidationProvider validationProvider,
                               ICodingChallengeRepository codingChallengeRepository,
                               IAssignmentToChallengeRepository assignmentRepository,
                               IUserDetailsProvider userDetails)
        {
            _testCaseRepository = testCaseRepository;
            _validationProvider = validationProvider;
            _codingChallengeRepository = codingChallengeRepository;
            _assignmentRepository = assignmentRepository;
            _userDetails = userDetails;
        }

        public async Task<TestCaseResponseDto> CreateAsync(TestCaseRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            return await _testCaseRepository.CreateAsync<TestCaseRequestDto, TestCaseResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _testCaseRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<TestCaseResponseDto>> GetPageAsync(TestCasesPagingInfo pagingInfo)
        {
            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                var codingChallengeIds = await _assignmentRepository.GetAssignedCodingChallengeIdsAsync(_userDetails.GetUserId());

                if (codingChallengeIds.Count == 0)
                {
                    return new PaginatedResult<TestCaseResponseDto>(new List<TestCaseResponseDto>(), 0, 0, 0);
                }

                pagingInfo.codingChallengeIds = codingChallengeIds;

                return await _testCaseRepository.GetPageAsync(pagingInfo);
            }

            return await _testCaseRepository.GetPageAsync<TestCaseResponseDto>(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task<TestCaseResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                var testCase = await _testCaseRepository.GetByIdAsync<TestCaseResponseDto>(id);

                if(! await IsAccessAllowed(testCase, _userDetails.GetUserId()))
                {
                    throw new UnauthorizedAccessException("You do not have access to this test case.");
                }

                 return testCase;
            }

            return await _testCaseRepository.GetByIdAsync<TestCaseResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, TestCaseRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            await _testCaseRepository.UpdateAsync(id, dto);
        }

        private async Task ValidateRequestDto(TestCaseRequestDto dto)
        {
            var codingChallengeExists = await _codingChallengeRepository.HasAnyAsync(dto.CodingChallengeId);

            if (!codingChallengeExists)
            {
                throw new NotFoundException("Invalid coding challenge ID.");
            }
        }

        private async Task<bool> IsAccessAllowed(TestCaseResponseDto testCase, int userId)
        {
            var allowed = true;

            var assignmentExists = await _assignmentRepository.HasAnyAsync(testCase.CodingChallengeId,
                                                                                               userId);
            if (assignmentExists)
            {
                var assignment = await _assignmentRepository.GetByInternAndChallengeIdsAsync(userId,
                                                                                             testCase.CodingChallengeId);
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
