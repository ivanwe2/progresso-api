using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;

namespace Prime.Progreso.Services
{
    public class AnswerChoiceService : IAnswerChoiceService
    {
        private readonly IAnswerChoiceRepository _answerChoiceRepository;
        private IValidationProvider _validationProvider;
        private readonly IQuizExecutionRepository _quizExecutionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserDetailsProvider _userDetails;

        public AnswerChoiceService(IAnswerChoiceRepository answerChoiceRepository,
                                   IValidationProvider validationProvider,
                                   IQuizExecutionRepository quizExecutionRepository,
                                   IQuestionRepository questionRepository,
                                   IUserDetailsProvider userDetailsHelper,
                                   IQuizRepository quizRepository)
        {
            _answerChoiceRepository = answerChoiceRepository;
            _validationProvider = validationProvider;
            _quizExecutionRepository = quizExecutionRepository;
            _questionRepository = questionRepository;
            _userDetails = userDetailsHelper;
            _quizRepository = quizRepository;
        }

        public async Task<AnswerChoiceResponseDto> CreateAsync(AnswerChoiceRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                await EnsureInternHasAccessAsync(dto.QuizExecutionId, _userDetails.GetUserId());
            }

            return await _answerChoiceRepository.CreateAsync<AnswerChoiceRequestDto, AnswerChoiceResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _answerChoiceRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<AnswerChoiceResponseDto>> GetPageAsync(
            int pageNumber, int pageSize)
        {
            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _answerChoiceRepository.GetPageAndFilterByUserIdAsync(pageNumber, pageSize, _userDetails.GetUserId());
            }

            return await _answerChoiceRepository.GetPageAsync<AnswerChoiceResponseDto>(pageNumber, pageSize);
        }

        public async Task<AnswerChoiceResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _answerChoiceRepository.GetByIdAsync(id, _userDetails.GetUserId());
            }

            return await _answerChoiceRepository.GetByIdAsync<AnswerChoiceResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, AnswerChoiceRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            await _answerChoiceRepository.UpdateAsync(id, dto);
        }

        private async Task ValidateRequestDto(AnswerChoiceRequestDto dto)
        {
            var quizExecution = await _quizExecutionRepository.GetByIdAsync<QuizExecutionResponseDto>(dto.QuizExecutionId);

            if (quizExecution is null)
            {
                throw new NotFoundException("Invalid quiz execution ID.");
            }

            var question = await _questionRepository.GetByIdAsync(dto.QuestionId);

            if (question is null)
            {
                throw new NotFoundException("Invalid question ID.");
            }

            if(!await _quizRepository.IsQuestionRelatedToQuizAsync(quizExecution.QuizId, dto.QuestionId))
            {
                throw new UnauthorizedAccessException("Question is not part of the Quiz you are executing!");
            }
            
            if (!question.Answers.Any(a => a.Id == dto.ChoiceId))
            {
                throw new NotFoundException("Invalid choice ID.");
            }
        }

        private async Task EnsureInternHasAccessAsync(Guid quizExecutionId, int userId)
        {
            if(!await _quizExecutionRepository.IsRelatedToUserAsync(quizExecutionId,userId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this QuizExecution!");
            }
        }
    }
}
