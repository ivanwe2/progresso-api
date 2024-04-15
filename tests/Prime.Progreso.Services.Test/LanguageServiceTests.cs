using Moq;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;
using Xunit;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using Prime.Progreso.Domain.Validators.Language;
using Prime.Progreso.Domain.Exceptions;
using FluentValidation.Results;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services.Test
{
    public class LanguageServiceTests
    {
        public ILanguageService languageService;
        public Mock<ILanguageRepository> languageRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public Mock<IValidationProvider> validationProviderMock;

        public LanguageServiceTests()
        {
            languageRepoMock = new Mock<ILanguageRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();

            languageRepoMock
                .Setup(s => s.CreateAsync<LanguageRequestDto, LanguageResponseDto>(It.IsAny<LanguageRequestDto>()))
                .ReturnsAsync(new LanguageResponseDto());
            languageRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            languageRepoMock
                .Setup(s => s.GetByIdAsync<LanguageResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new LanguageResponseDto());
            languageRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<LanguageRequestDto>()));

            validatorFactoryMock
                .Setup(s => s.GetValidator<LanguageRequestDto>())
                .Returns(new LanguageRequestDtoValidator());

            validationProviderMock = new Mock<IValidationProvider>();

            languageService = new LanguageService(languageRepoMock.Object, validationProviderMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var languageRequestDto = new LanguageRequestDto();

            var expected = new LanguageResponseDto();

            // Act
            var result = await languageService.CreateAsync(languageRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            languageRepoMock.Verify(r => r.CreateAsync<LanguageRequestDto, LanguageResponseDto>(languageRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidateAsync(It.IsAny<LanguageRequestDto>()))
                .ThrowsAsync(new ValidationException("sth"));

            languageService = new LanguageService(null, validationProviderMock.Object);

            // Act
            async Task test() => await languageService.CreateAsync(new LanguageRequestDto());

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new LanguageResponseDto() { Id = id };

            languageRepoMock
                .Setup(s => s.GetByIdAsync<LanguageResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new LanguageResponseDto() { Id = id });

            languageService = new LanguageService(languageRepoMock.Object, validationProviderMock.Object);

            // Act
            var result = await languageService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            languageRepoMock.Verify(r => r.GetByIdAsync<LanguageResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidLanguageId_ExpectedException()
        {
            // Act
            async Task test() => await languageService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<LanguageResponseDto>(new List<LanguageResponseDto>(), 0, 0, 0);

            languageRepoMock
                .Setup(s => s.GetPageAsync<LanguageResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var languages = await languageService.GetPageAsync(1, 10, null);

            // Assert
            Assert.NotNull(languages);
            Assert.Empty(languages.Content);
            languageRepoMock.Verify(r => r.GetPageAsync<LanguageResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var languageRequestDto = new LanguageRequestDto();

            // Act
            await languageService.UpdateAsync(id, languageRequestDto);

            // Assert
            languageRepoMock.Verify(r => r.UpdateAsync<LanguageRequestDto>(id, languageRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidLanguageId_ExpectedException()
        {
            // Act
            async Task test() => await languageService.UpdateAsync(default(Guid), new LanguageRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await languageService.DeleteAsync(id);

            // Assert
            languageRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidLanguageId_ExpectedException()
        {
            // Act
            async Task a() => await languageService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
