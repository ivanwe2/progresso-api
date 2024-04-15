using FluentValidation.Results;
using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.Question;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly Mock<IQuestionCategoryRepository> _questionCategoryRepositoryMock;
        private readonly Mock<IValidatorFactory> _validatorFactoryMock;
        private readonly Mock<IValidationProvider> _validationProvider;

        public QuestionServiceTests()
        {
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _questionCategoryRepositoryMock = new Mock<IQuestionCategoryRepository>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();

            _validatorFactoryMock
                .Setup(s => s.GetValidator<QuestionRequestDto>())
                .Returns(new QuestionRequestDtoValidator());

            _validationProvider = new Mock<IValidationProvider>();
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuestionRequestDto();

            _validationProvider
                .Setup(s => s.TryValidateAsync(It.IsAny<QuestionRequestDto>()))
                .Throws(new ValidationException("invalid data"));

            var service = new QuestionService(null, null, _validationProvider.Object);

            // Act
            async Task a() => await service.CreateAsync(requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task CreateAsync_ValidData_CreatedQuestionReturned()
        {
            // Arrange
            var requestDto = new QuestionRequestDto
            {
                Title = "title",
                Answers = new List<AnswerRequestDto> {
                    new AnswerRequestDto { Content = "", IsCorrect = true },
                    new AnswerRequestDto { Content = "", IsCorrect = false },
                    new AnswerRequestDto { Content = "", IsCorrect = false },
                    new AnswerRequestDto { Content = "", IsCorrect = false }
                },
                QuestionCategoryIds = new List<Guid> { Guid.NewGuid() }
            };

            var responseDto = new QuestionResponseDto();

            _questionCategoryRepositoryMock.Setup(s => s.DoAllCategoriesExistAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);
           
            _questionRepositoryMock
                .Setup(r => r.CreateAsync<QuestionRequestDto, QuestionResponseDto>(It.IsAny<QuestionRequestDto>()))
                .ReturnsAsync(responseDto);

            var service = new QuestionService(_questionRepositoryMock.Object,
                                              _questionCategoryRepositoryMock.Object,
                                              _validationProvider.Object);

            // Act
            var result = await service.CreateAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            _questionRepositoryMock.Verify(r => r.CreateAsync<QuestionRequestDto, QuestionResponseDto>(requestDto),
                                                   Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidQuestionId_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var questionId = Guid.NewGuid();

            var service = new QuestionService(_questionRepositoryMock.Object, null, null);

            // Act
            await service.DeleteAsync(questionId);

            // Assert
            _questionRepositoryMock.Verify(r => r.DeleteAsync(questionId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ValidQuestionId_QuestionReturned()
        {
            // Arrange
            var questionId = Guid.NewGuid();

            var responseDto = new QuestionResponseDto
            {
                Id = questionId
            };

            _questionRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(responseDto);

            var service = new QuestionService(_questionRepositoryMock.Object, null, null);

            // Act
            var result = await service.GetByIdAsync(questionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            Assert.Equal(questionId, result.Id);
            _questionRepositoryMock.Verify(r => r.GetByIdAsync(questionId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuestionRequestDto();

            var id = Guid.NewGuid();

            _validationProvider
                .Setup(s => s.TryValidateAsync(It.IsAny<QuestionRequestDto>()))
                .Throws(new ValidationException("invalid data"));

            var service = new QuestionService(null, null, _validationProvider.Object);

            // Act
            async Task a() => await service.UpdateAsync(id, requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var requestDto = new QuestionRequestDto
            {
                Title = "title",
                Answers = new List<AnswerRequestDto> {
                    new AnswerRequestDto { Content = "", IsCorrect = true },
                    new AnswerRequestDto { Content = "", IsCorrect = false },
                    new AnswerRequestDto { Content = "", IsCorrect = false },
                    new AnswerRequestDto { Content = "", IsCorrect = false }
                },
                QuestionCategoryIds = new List<Guid> { Guid.NewGuid() }
            };

            var id = Guid.NewGuid();
            _questionCategoryRepositoryMock.Setup(s => s.DoAllCategoriesExistAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);

            var service = new QuestionService(_questionRepositoryMock.Object,
                                              _questionCategoryRepositoryMock.Object,
                                              _validationProvider.Object);

            // Act
            await service.UpdateAsync(id, requestDto);

            // Assert
            _questionRepositoryMock.Verify(r => r.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task GetPageAsync_ValidParameter_PaginatedResultReturned()
        {
            // Arrange
            var paginatedResult = new PaginatedResult<QuestionResponseDto>(new List<QuestionResponseDto>(), 0, 0, 0);

            _questionRepositoryMock.Setup(r => r.GetPageAsync<QuestionResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(paginatedResult);

            var service = new QuestionService(_questionRepositoryMock.Object, null, null);

            // Act
            var result = await service.GetPageAsync(0, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginatedResult, result);
            _questionRepositoryMock.Verify(r => r.GetPageAsync<QuestionResponseDto>(0, 0, null), Times.Once);
        }
    }
}
