using Newtonsoft.Json;
using Prime.Progreso.Domain.Abstractions.Helpers;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Quiz;

namespace Prime.Progreso.Services
{
    public class QuizService : IQuizService
    {
        private const string getSeasonUrl = "https://int-team.protal.biz/progreso/dev/java-api/seasons";
        private readonly IQuizRepository _quizRepository;
        private readonly IAnswerChoiceRepository _answerChoiceRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizAssignmentRepository _quizAssignmentRepository;
        private readonly IValidationProvider _validationProvider;
        private readonly IQuizExecutionRepository _quizExecutionRepository;
        private readonly IJavaApiHelper _javaApiHelper;
        private readonly IUserDetailsProvider _userDetails;

        public QuizService(IQuizRepository quizRepository,
                           IQuestionRepository questionRepository,
                           IAnswerChoiceRepository answerChoiceRepository,
                           IValidationProvider validationProvider,
                           IQuizAssignmentRepository quizAssignmentRepository,
                           IUserDetailsProvider userDetails,
                           IJavaApiHelper javaApiHelper,
                           IQuizExecutionRepository quizExecutionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _validationProvider = validationProvider;
            _quizAssignmentRepository = quizAssignmentRepository;
            _userDetails = userDetails;
            _answerChoiceRepository = answerChoiceRepository;
            _javaApiHelper = javaApiHelper;
            _quizExecutionRepository = quizExecutionRepository;
        }
        public async Task<QuizResponseDto> CreateAsync(QuizRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateQuestionsAsync(dto);

            return await _quizRepository.CreateAsync<QuizRequestDto, QuizResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _quizRepository.DeleteAsync(id);
        }

        public async Task<QuizResponseDto> GetByIdAsync(Guid id)
        {
            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                await EnsureInternIsAssignedToQuizAsync(_userDetails.GetUserId(), id);
            }    

            return await _quizRepository.GetByIdAsync(id);
        }

        public async Task<PaginatedResult<QuizResponseDto>> GetPageAsync(QuizesPagingInfo pagingInfo)
        {
            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                pagingInfo.quizIds = await _quizAssignmentRepository
                    .GetQuizIdsAssignedToUser(new PagingInfo() { Page = pagingInfo.Page, Size = pagingInfo.Size},
                    _userDetails.GetUserId(), DateTime.UtcNow);

                return await _quizRepository.GetPageByFilterAsync(pagingInfo);
            }

            return await _quizRepository.GetPageAsync<QuizResponseDto>(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task UpdateAsync(Guid id, QuizRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateQuestionsAsync(dto);
            
            await _quizRepository.UpdateAsync(id, dto);
        }

        public async Task<QuizzesStatisticsPaginatedDto> GetPagedStatisticsAsync(QuizStatisticsPagingInfo pagingInfo)
        {
            await CheckIfUsersBelongsToSeason(pagingInfo.userIds, pagingInfo.seasonIds);

            var quizzes = await _quizExecutionRepository.GetAllQuizStatisticsAsync(pagingInfo);

            var result = new List<QuizStatisticsResponseDto>();

            if (quizzes.Count > 0)
            {
                foreach (var quiz in quizzes)
                {
                    var questionStatistics = await GetStatisticsAsync(quiz.Id);

                    result.Add(new QuizStatisticsResponseDto
                    {
                        Id = quiz.Id,
                        Title = quiz.Title,
                        Questions = questionStatistics,
                        CompletionRate = Math.Round(questionStatistics.Average(x => x.CompletionRate), 2)
                    });
                }
            }            

            return new QuizzesStatisticsPaginatedDto 
            {
                Quizzes = new PaginatedResult<QuizStatisticsResponseDto>(result, result.Count, 0, 10),
                CompletionRate = result.Any() ? Math.Round(result.Average(x => x.CompletionRate), 2) : 0
            };
        }

        public async Task<QuizStatisticsResponseDto> GetStatisticsByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var quiz = await _quizRepository.GetByIdAsync(id);

            var questionStatistics = await GetStatisticsAsync(id);

            return new QuizStatisticsResponseDto
            {
                Id = id,
                Title = quiz.Title,
                Questions = questionStatistics,
                CompletionRate = questionStatistics.Any() ? Math.Round(questionStatistics.Average(x => x.CompletionRate), 2) : 0
            };
        }

        private async Task ValidateQuestionsAsync(QuizRequestDto dto)
        {
            var isValid = await _questionRepository.DoAllQuestionsExist(dto.QuestionIds);

            if (!isValid)
            {
                throw new NotFoundException("One or more question IDs are invalid.");
            }
        }

        private async Task EnsureInternIsAssignedToQuizAsync(int userId, Guid quizId)
        {
            if (!await _quizAssignmentRepository.IsInternAssignedToQuizAsync(quizId, userId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Quiz!");
            }
        }

        private async Task<List<QuestionStatisticsResponseDto>> GetStatisticsAsync(Guid quizId)
        {
            var questionIds = await _questionRepository.GetQuestionIdsByQuizIdAsync(quizId);

            var questions = await _answerChoiceRepository.GetStatisticsByQuizAndQuestionIdsAsync(quizId, questionIds);

            return await _questionRepository.CalculateSuccessRateForQuestionStatisticsAsync(questions);
        }

        private async Task<object> GetSeasonFromJavaApi(int seasonId)
        {
            var client = new HttpClient();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{getSeasonUrl}/{seasonId}"))
            {
                request.Headers.Add(_javaApiHelper.ApiKey, _javaApiHelper.XApiKeyValue);
                request.Headers.Add(_javaApiHelper.LoggedUserEmail, _javaApiHelper.AdminEmail);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new NotFoundException("Season was not found!");
                }

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(result);
            }           
        }

        private async Task CheckIfUsersBelongsToSeason(List<int> userIds, List<int> seasonIds)
        {
            if (seasonIds is not null && seasonIds.Any())
            {
                foreach (var seasonId in seasonIds)
                {
                    dynamic season = await GetSeasonFromJavaApi(seasonId);

                    bool isInternFromSeason = false;

                    foreach (var intern in season.interns)
                    {
                        if (userIds.Any(x => x == intern.id.Value))
                            isInternFromSeason = true;
                    }
                    if (!isInternFromSeason)
                    {
                        throw new NotFoundException("Some user Ids do not belong to the season Ids");
                    }
                }
            }           
        }
    }
}
