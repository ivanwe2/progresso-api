using Microsoft.Extensions.Options;
using Moq;
using Prime.Progreso.Domain.Abstractions.Helpers;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Quiz;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _quizRepositoryMock;
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly Mock<IValidationProvider> _validationProvider;
        private readonly Mock<IQuizAssignmentRepository> _quizAssignmentRepositoryMock;
        private readonly Mock<IUserDetailsProvider> _userDetailsProviderMock;
        private readonly Mock<IQuizExecutionRepository> _quizExecutionRepositoryMock;
        private readonly Mock<IAnswerChoiceRepository> _answerChoiceRepositoryMock;
        private readonly Mock<IJavaApiHelper> _javaApiHelper;

        public QuizServiceTests()
        {
            _quizRepositoryMock = new Mock<IQuizRepository>();
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _validationProvider = new Mock<IValidationProvider>();
            _quizAssignmentRepositoryMock = new Mock<IQuizAssignmentRepository>();
            _userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            _answerChoiceRepositoryMock = new Mock<IAnswerChoiceRepository>();
            _quizExecutionRepositoryMock = new Mock<IQuizExecutionRepository>();
            _javaApiHelper = new Mock<IJavaApiHelper>();
        }


        [Fact]
        public async Task CreateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuizRequestDto
            {
                QuestionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            _validationProvider
                .Setup(s => s.TryValidate(It.IsAny<QuizRequestDto>()))
                .Throws(new ValidationException("invalid data"));

            _questionRepositoryMock.Setup(r => r.DoAllQuestionsExist(It.IsAny<List<Guid>>())).ReturnsAsync(true);

            var service = new QuizService(null, null, null, _validationProvider.Object, null, null, _javaApiHelper.Object, null);

            // Act
            async Task a() => await service.CreateAsync(requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task CreateAsync_ValidData_CreatedQuizReturned()
        {
            // Arrange
            var requestDto = new QuizRequestDto
            {
                Title = "Title",
                Duration = 5,
                QuestionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var responseDto = new QuizResponseDto();

            _questionRepositoryMock.Setup(r => r.DoAllQuestionsExist(It.IsAny<List<Guid>>())).ReturnsAsync(true);

            _quizRepositoryMock
                .Setup(r => r.CreateAsync<QuizRequestDto, QuizResponseDto>(It.IsAny<QuizRequestDto>()))
                .ReturnsAsync(responseDto);

            var service = new QuizService(_quizRepositoryMock.Object,
                                          _questionRepositoryMock.Object,
                                          null,
                                          _validationProvider.Object, null, null, _javaApiHelper.Object, null);

            // Act
            var result = await service.CreateAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            _quizRepositoryMock.Verify(r => r.CreateAsync<QuizRequestDto, QuizResponseDto>(requestDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidQuizId_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var quizId = Guid.NewGuid();

            var service = new QuizService(_quizRepositoryMock.Object, null, null, null, null, null, _javaApiHelper.Object, null);

            // Act
            await service.DeleteAsync(quizId);

            // Assert
            _quizRepositoryMock.Verify(r => r.DeleteAsync(quizId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ValidQuizId_QuizReturned()
        {
            // Arrange
            var quizId = Guid.NewGuid();

            var responseDto = new QuizResponseDto
            {
                Id = quizId
            };

            _quizRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(responseDto);

            _quizAssignmentRepositoryMock
                .Setup(s => s.IsInternAssignedToQuizAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new QuizService(_quizRepositoryMock.Object, null, null, null, _quizAssignmentRepositoryMock.Object,
                _userDetailsProviderMock.Object, _javaApiHelper.Object, null);

            // Act
            var result = await service.GetByIdAsync(quizId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            Assert.Equal(quizId, result.Id);
            _quizRepositoryMock.Verify(r => r.GetByIdAsync(quizId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuizRequestDto
            {
                QuestionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var id = Guid.NewGuid();

            _validationProvider
                .Setup(s => s.TryValidate(It.IsAny<QuizRequestDto>()))
                .Throws(new ValidationException("invalid data"));

            var service = new QuizService(null, null, null, _validationProvider.Object, null, null, _javaApiHelper.Object, null);

            // Act
            async Task a() => await service.UpdateAsync(id, requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var requestDto = new QuizRequestDto
            {
                Title = "Title",
                Duration = 5,
                QuestionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var id = Guid.NewGuid();
            _questionRepositoryMock.Setup(r => r.DoAllQuestionsExist(It.IsAny<List<Guid>>())).ReturnsAsync(true);

            var service = new QuizService(_quizRepositoryMock.Object,
                                          _questionRepositoryMock.Object,
                                          null,
                                          _validationProvider.Object,
                                          null, null, _javaApiHelper.Object, null);

            // Act
            await service.UpdateAsync(id, requestDto);

            // Assert
            _quizRepositoryMock.Verify(r => r.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task GetPageAsync_ValidParameter_RoleAdmin_PaginatedResultReturned()
        {
            // Arrange
            var paginatedResult = new PaginatedResult<QuizResponseDto>(new List<QuizResponseDto>(), 0, 0, 0);
            var pagingInfo = new QuizesPagingInfo();

            string role = "ROLE_ADMIN";
            int userId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            _quizRepositoryMock.Setup(r => r.GetPageAsync<QuizResponseDto>(pagingInfo.Page, pagingInfo.Size, null))
                .ReturnsAsync(paginatedResult);

            var service = new QuizService(_quizRepositoryMock.Object, null, null, null, null,
                _userDetailsProviderMock.Object, _javaApiHelper.Object, null);

            // Act
            var result = await service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginatedResult, result);
            _quizRepositoryMock.Verify(r => r.GetPageAsync<QuizResponseDto>(pagingInfo.Page,pagingInfo.Size,null), Times.Once);
        }

        [Fact]
        public async Task GetPageAsync_ValidParameter_RoleIntern_PaginatedResultReturned()
        {
            // Arrange
            var paginatedResult = new PaginatedResult<QuizResponseDto>(new List<QuizResponseDto>(), 0, 0, 0);
            var pagingInfo = new QuizesPagingInfo();

            string role = "ROLE_INTERN";
            int userId = 10;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            _quizRepositoryMock.Setup(r => r.GetPageByFilterAsync(It.IsAny<QuizesPagingInfo>()))
                .ReturnsAsync(paginatedResult);

            var service = new QuizService(_quizRepositoryMock.Object, null, null, null, _quizAssignmentRepositoryMock.Object,
                _userDetailsProviderMock.Object, _javaApiHelper.Object, null);

            // Act
            var result = await service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginatedResult, result);
            _quizRepositoryMock.Verify(r => r.GetPageByFilterAsync(It.IsAny<QuizesPagingInfo>()), Times.Once);
        }

        [Fact]
        public async Task GetStatisticsByIdAsync_InvalidQuizId_ExpectedException()
        {
            // Arrange
            var service = new QuizService(_quizRepositoryMock.Object, null, null, null, 
                _quizAssignmentRepositoryMock.Object,
                _userDetailsProviderMock.Object, _javaApiHelper.Object, null);

            // Act
            async Task test() => await service.GetStatisticsByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetStatisticsByIdAsync_ValidData_ExpectedException()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            var questions = new List<QuestionStatisticsResponseDto>();

            var expected = new QuizStatisticsResponseDto
            {
                Id = id,
                Questions = questions,
                CompletionRate = questions.Any() ? Math.Round(questions.Average(x => x.CompletionRate), 2) : 0
            };            

            _questionRepositoryMock
                .Setup(r => r.GetQuestionIdsByQuizIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Guid>());
            _questionRepositoryMock
                .Setup(r => r.CalculateSuccessRateForQuestionStatisticsAsync(It.IsAny<List<QuestionStatisticsResponseDto>>()))
                .ReturnsAsync(questions);

            _quizRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizResponseDto { Id = id });

            _answerChoiceRepositoryMock
                .Setup(r => r.GetStatisticsByQuizAndQuestionIdsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(questions);

            var service = new QuizService(_quizRepositoryMock.Object, _questionRepositoryMock.Object, 
                _answerChoiceRepositoryMock.Object, null,
                _quizAssignmentRepositoryMock.Object,
                null, _javaApiHelper.Object, null);

            // Act
            var result = await service.GetStatisticsByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _quizRepositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetPagedStatisticsAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<QuizExecutionResponseDto>(new List<QuizExecutionResponseDto>(), 0, 0, 0);

            var pagingInfo = new QuizStatisticsPagingInfo
            {
                seasonIds = new List<int>(),
                userIds = new List<int>()
            };

            var quizStatistics = new List<QuizStatisticsResponseDto>();

            _quizExecutionRepositoryMock
                  .Setup(s => s.GetAllQuizStatisticsAsync(It.IsAny<QuizStatisticsPagingInfo>()))
                  .ReturnsAsync(quizStatistics);

            var service = new QuizService(_quizRepositoryMock.Object, _questionRepositoryMock.Object,
                _answerChoiceRepositoryMock.Object, null,
                _quizAssignmentRepositoryMock.Object,
                null, _javaApiHelper.Object, _quizExecutionRepositoryMock.Object);

            // Act
            var result = await service.GetPagedStatisticsAsync(pagingInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Quizzes.Content);
            _quizExecutionRepositoryMock.Verify(r => r.GetAllQuizStatisticsAsync(pagingInfo), Times.Once());
        }
    }
}
