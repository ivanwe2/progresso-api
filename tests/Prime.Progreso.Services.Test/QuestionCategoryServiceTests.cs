using FluentValidation.Results;
using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.QuestionCategory;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class QuestionCategoryServiceTests
    {
        private readonly Mock<IQuestionCategoryRepository> _questionCategoryRepositoryMock;
        private readonly Mock<IValidatorFactory> _validatorFactoryMock;
        private IValidationProvider _validationProvider;

        public QuestionCategoryServiceTests()
        {
            _questionCategoryRepositoryMock = new Mock<IQuestionCategoryRepository>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();

            _validatorFactoryMock
                .Setup(s => s.GetValidator<QuestionCategoryRequestDto>())
                .Returns(new QuestionCategoryRequestDtoValidator());

            _validationProvider = new ValidationProvider(_validatorFactoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuestionCategoryRequestDto
            {
                Description = "description"
            };

            var service = new QuestionCategoryService(null, _validationProvider);

            // Act
            async Task a() => await service.CreateAsync(requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task CreateAsync_ValidData_CreatedQuestionCategoryReturned()
        {
            // Arrange
            var requestDto = new QuestionCategoryRequestDto
            {
                Title = "title",
                Description = "description"
            };

            var responseDto = new QuestionCategoryResponseDto();

            _questionCategoryRepositoryMock
                .Setup(r => r.CreateAsync<QuestionCategoryRequestDto, QuestionCategoryResponseDto>(It.IsAny<QuestionCategoryRequestDto>()))
                .ReturnsAsync(responseDto);

            var service = new QuestionCategoryService(_questionCategoryRepositoryMock.Object, _validationProvider);

            // Act
            var result = await service.CreateAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            _questionCategoryRepositoryMock.Verify(r => r.CreateAsync<QuestionCategoryRequestDto, QuestionCategoryResponseDto>(requestDto),
                                                   Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidQuestionCategoryId_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var service = new QuestionCategoryService(_questionCategoryRepositoryMock.Object, null);

            // Act
            await service.DeleteAsync(categoryId);

            // Assert
            _questionCategoryRepositoryMock.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ValidQuestionCategoryId_QuestionCategoryReturned()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var responseDto = new QuestionCategoryResponseDto
            {
                Id = categoryId
            };

            _questionCategoryRepositoryMock.Setup(r => r.GetByIdAsync<QuestionCategoryResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(responseDto);

            var service = new QuestionCategoryService(_questionCategoryRepositoryMock.Object, null);

            // Act
            var result = await service.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            Assert.Equal(categoryId, result.Id);
            _questionCategoryRepositoryMock.Verify(r => r.GetByIdAsync<QuestionCategoryResponseDto>(categoryId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ValidationExceptionThrown()
        {
            // Arrange
            var requestDto = new QuestionCategoryRequestDto
            {
                Description = "description"
            };

            var id = Guid.NewGuid();

            var service = new QuestionCategoryService(null, _validationProvider);

            // Act
            async Task a() => await service.UpdateAsync(id, requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_RepositoryMethodInvokedOnce()
        {
            // Arrange
            var requestDto = new QuestionCategoryRequestDto
            {
                Title = "title",
                Description = "description"
            };

            var id = Guid.NewGuid();

            var service = new QuestionCategoryService(_questionCategoryRepositoryMock.Object, _validationProvider);

            // Act
            await service.UpdateAsync(id, requestDto);

            // Assert
            _questionCategoryRepositoryMock.Verify(r => r.UpdateAsync<QuestionCategoryRequestDto>(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task GetPageAsync_ValidParameter_PaginatedResultReturned()
        {
            // Arrange
            var paginatedResult = new PaginatedResult<QuestionCategoryResponseDto>(new List<QuestionCategoryResponseDto>(), 0, 0, 0);

            _questionCategoryRepositoryMock.Setup(r => r.GetPageAsync<QuestionCategoryResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(paginatedResult);

            var service = new QuestionCategoryService(_questionCategoryRepositoryMock.Object, null);

            // Act
            var result = await service.GetPageAsync(0, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginatedResult, result);
            _questionCategoryRepositoryMock.Verify(r => r.GetPageAsync<QuestionCategoryResponseDto>(0, 0, null), Times.Once);
        }
    }
}
