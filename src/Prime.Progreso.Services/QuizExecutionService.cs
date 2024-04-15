using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Constants;

namespace Prime.Progreso.Services
{
    public class QuizExecutionService : IQuizExecutionService
    {
        private readonly IQuizExecutionRepository _quizExecutionRepo;
        private IValidationProvider _validationProvider;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizAssignmentRepository _quizAssignmentRepository;
        private readonly IUserDetailsProvider _userDetails;

        public QuizExecutionService(IQuizExecutionRepository quizExecutionRepo,
                                    IValidationProvider validationProvider,
                                    IQuizRepository quizRepository,
                                    IQuizAssignmentRepository quizAssignmentRepository,
                                    IUserDetailsProvider userDetailsHelper)
        {
            _quizExecutionRepo = quizExecutionRepo;
            _validationProvider = validationProvider;
            _quizRepository = quizRepository;
            _quizAssignmentRepository = quizAssignmentRepository;
            _userDetails = userDetailsHelper;
        }

        public async Task<QuizExecutionResponseDto> CreateAsync(QuizExecutionRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateRequestDtoAsync(dto);

            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                await EnsureInternIsAssignedToQuizAsync(_userDetails.GetUserId(), dto.QuizId);
                dto.UserId = _userDetails.GetUserId();
            }

            return await _quizExecutionRepo.CreateAsync<QuizExecutionRequestDto, QuizExecutionResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _quizExecutionRepo.DeleteAsync(id);
        }

        public async Task<PaginatedResult<QuizExecutionResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _quizExecutionRepo.GetPageAndFilterByUserIdAsync(pageNumber, pageSize, _userDetails.GetUserId());
            }

            return await _quizExecutionRepo.GetPageAsync<QuizExecutionResponseDto>(pageNumber, pageSize);
        }

        public async Task<QuizExecutionResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _quizExecutionRepo.GetByIdAsync(id, _userDetails.GetUserId());
            }

            return await _quizExecutionRepo.GetByIdAsync<QuizExecutionResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, QuizExecutionRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await ValidateRequestDtoAsync(dto);

            await _quizExecutionRepo.UpdateAsync(id, dto);
        }

        private async Task ValidateRequestDtoAsync(QuizExecutionRequestDto dto)
        {
            var quizExists = await _quizRepository.HasAnyAsync(dto.QuizId);

            if (!quizExists)
            {
                throw new NotFoundException("Invalid quiz ID.");
            }
        }

        private async Task EnsureInternIsAssignedToQuizAsync(int userId, Guid quizId)
        {
            if(!await _quizAssignmentRepository.IsInternAssignedToQuizAsync(quizId,userId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Quiz!");
            }
        }
    }
}
