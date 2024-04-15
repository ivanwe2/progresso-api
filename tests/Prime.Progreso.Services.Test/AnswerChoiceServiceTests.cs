using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators.AnswerChoice;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class AnswerChoiceServiceTests
    {
        public IAnswerChoiceService answerChoiceService;
        public Mock<IAnswerChoiceRepository> answerChoiceRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public Mock<IQuestionRepository> questionRepoMock;
        public Mock<IQuizExecutionRepository> quizExecutionRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<IUserDetailsProvider> userDetailsProviderMock;
        public Mock<IQuizRepository> quizRepoMock;

        public AnswerChoiceServiceTests()
        {
            answerChoiceRepoMock = new Mock<IAnswerChoiceRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();
            questionRepoMock = new Mock<IQuestionRepository>();
            quizExecutionRepoMock = new Mock<IQuizExecutionRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            userDetailsProviderMock = new();
            quizRepoMock = new();

            answerChoiceRepoMock
                .Setup(s => s.CreateAsync<AnswerChoiceRequestDto, AnswerChoiceResponseDto>(
                    It.IsAny<AnswerChoiceRequestDto>()))
                .ReturnsAsync(new AnswerChoiceResponseDto());
            answerChoiceRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            answerChoiceRepoMock
                .Setup(s => s.GetByIdAsync<AnswerChoiceResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new AnswerChoiceResponseDto());
            answerChoiceRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<AnswerChoiceRequestDto>()));

            validatorFactoryMock
                .Setup(s => s.GetValidator<AnswerChoiceRequestDto>())
                .Returns(new AnswerChoiceRequestDtoValidator());

            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync<QuizExecutionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizExecutionResponseDto());

            questionRepoMock
                .Setup(s => s.GetByIdAsync<QuestionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuestionResponseDto());

            quizExecutionRepoMock
                .Setup(s=> s.IsRelatedToUserAsync(It.IsAny<Guid>(),It.IsAny<int>()))
                .ReturnsAsync(true);

            quizRepoMock
                .Setup(s => s.IsQuestionRelatedToQuizAsync(It.IsAny<Guid>(),It.IsAny<Guid>()))
                .ReturnsAsync(true);

            answerChoiceService = new AnswerChoiceService(answerChoiceRepoMock.Object,
                validationProviderMock.Object, quizExecutionRepoMock.Object, questionRepoMock.Object, 
                userDetailsProviderMock.Object, quizRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var choiceId = Guid.NewGuid();

            var answerChoiceRequestDto = new AnswerChoiceRequestDto()
            {
                ChoiceId = choiceId
            };

            var expected = new AnswerChoiceResponseDto();

            questionRepoMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new QuestionResponseDto()
                { 
                    Answers = new List<AnswerResponseDto>() 
                    {
                        new AnswerResponseDto() { Id =  choiceId } 
                    }
                });

            answerChoiceService = new AnswerChoiceService(answerChoiceRepoMock.Object, validationProviderMock.Object, 
                quizExecutionRepoMock.Object, questionRepoMock.Object, userDetailsProviderMock.Object, quizRepoMock.Object);

            // Act
            var result = await answerChoiceService.CreateAsync(answerChoiceRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            answerChoiceRepoMock.Verify(r =>
                r.CreateAsync<AnswerChoiceRequestDto, AnswerChoiceResponseDto>(answerChoiceRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedValidationException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<AnswerChoiceRequestDto>()))
                .Throws(new ValidationException("sth"));

            answerChoiceService = new AnswerChoiceService(null, validationProviderMock.Object, null, null, null, null);

            // Act
            async Task test() => await answerChoiceService.CreateAsync(new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task CreateAsync_InvalidQuizExecutionId_ExpectedNotFoundException()
        {
            // Arrange
            QuizExecutionResponseDto nullDto = null;
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<AnswerChoiceRequestDto>()));

            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync<QuizExecutionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(nullDto);

            answerChoiceService = new AnswerChoiceService(null, validationProviderMock.Object, 
                quizExecutionRepoMock.Object, null, null, null);

            // Act
            async Task test() => await answerChoiceService.CreateAsync(new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task CreateAsync_InvalidQuestionId_ExpectedNotFoundException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<AnswerChoiceRequestDto>()));

            questionRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            answerChoiceService = new AnswerChoiceService(null, validationProviderMock.Object, 
                quizExecutionRepoMock.Object, questionRepoMock.Object, null, null);

            // Act
            async Task test() => await answerChoiceService.CreateAsync(new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithInternRole_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new AnswerChoiceResponseDto() { Id = id };
            string role = "ROLE_INTERN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            answerChoiceRepoMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), userId))
                .ReturnsAsync(new AnswerChoiceResponseDto() { Id = id });

            answerChoiceService = new AnswerChoiceService(answerChoiceRepoMock.Object, validationProviderMock.Object, null
                , null, userDetailsProviderMock.Object, null);

            // Act
            var result = await answerChoiceService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            answerChoiceRepoMock.Verify(r => r.GetByIdAsync(id, userId), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithAdminOrMentorRole_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new AnswerChoiceResponseDto() { Id = id };
            string role = "ROLE_ADMIN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            answerChoiceRepoMock
                .Setup(s => s.GetByIdAsync<AnswerChoiceResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new AnswerChoiceResponseDto() { Id = id });

            answerChoiceService = new AnswerChoiceService(answerChoiceRepoMock.Object, validationProviderMock.Object, null
                , null, userDetailsProviderMock.Object, null);

            // Act
            var result = await answerChoiceService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            answerChoiceRepoMock.Verify(r => r.GetByIdAsync<AnswerChoiceResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidAnswerChoiceId_ExpectedException()
        {
            // Act
            async Task test() => await answerChoiceService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithInternRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<AnswerChoiceResponseDto>(new List<AnswerChoiceResponseDto>(), 0, 0, 0);
            string role = "ROLE_INTERN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            answerChoiceRepoMock
                  .Setup(s => s.GetPageAndFilterByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                  .ReturnsAsync(page);

            // Act
            var answerChoices = await answerChoiceService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(answerChoices);
            Assert.Empty(answerChoices.Content);
            answerChoiceRepoMock.Verify(r => r.GetPageAndFilterByUserIdAsync(1, 10, userId), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithAdminOrMentor_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<AnswerChoiceResponseDto>(new List<AnswerChoiceResponseDto>(), 0, 0, 0);
            string role = "ROLE_ADMIN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            answerChoiceRepoMock
                .Setup(s => s.GetPageAsync<AnswerChoiceResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var answerChoices = await answerChoiceService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(answerChoices);
            Assert.Empty(answerChoices.Content);
            answerChoiceRepoMock.Verify(r => r.GetPageAsync<AnswerChoiceResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var choiceId = Guid.NewGuid();

            var answerChoiceRequestDto = new AnswerChoiceRequestDto()
            {
                ChoiceId = choiceId
            };

            questionRepoMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new QuestionResponseDto()
                {
                    Answers = new List<AnswerResponseDto>()
                    {
                        new AnswerResponseDto() { Id =  choiceId }
                    }
                });

            answerChoiceService = new AnswerChoiceService(answerChoiceRepoMock.Object, validationProviderMock.Object,
                quizExecutionRepoMock.Object, questionRepoMock.Object, null, quizRepoMock.Object);

            // Act
            await answerChoiceService.UpdateAsync(id, answerChoiceRequestDto);

            // Assert
            answerChoiceRepoMock.Verify(r => r.UpdateAsync<AnswerChoiceRequestDto>(id, answerChoiceRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidAnswerChoiceId_ExpectedException()
        {
            // Act
            async Task test() => await answerChoiceService.UpdateAsync(default(Guid), new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidQuizExecutionId_ExpectedNotFoundException()
        {
            // Arrange
            QuizExecutionResponseDto quizExecNull = null;
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<AnswerChoiceRequestDto>()));

            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync<QuizExecutionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(quizExecNull);

            answerChoiceService = new AnswerChoiceService(null, validationProviderMock.Object,
                quizExecutionRepoMock.Object, null, null, null);

            // Act
            async Task test() => await answerChoiceService.UpdateAsync(Guid.NewGuid(), new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidQuestionId_ExpectedNotFoundException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<QuizExecutionRequestDto>()));

            questionRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            answerChoiceService = new AnswerChoiceService(null, validationProviderMock.Object, 
                quizExecutionRepoMock.Object, questionRepoMock.Object, null, null);

            // Act
            async Task test() => await answerChoiceService.UpdateAsync(Guid.NewGuid(), new AnswerChoiceRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        // same

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await answerChoiceService.DeleteAsync(id);

            // Assert
            answerChoiceRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidAnswerChoiceId_ExpectedException()
        {
            // Act
            async Task a() => await answerChoiceService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
