using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using Prime.Progreso.Domain.Enums;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators.Keyword;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class KeywordServiceTests
    {
        private readonly Mock<IKeywordRepository> _repositoryMock;
        private readonly Mock<IValidatorFactory> _validatorFactory;
        private readonly Mock<IValidationProvider> _validationProviderMock;
        private readonly Mock<ILanguageRepository> _languageRepositoryMock;

        private IKeywordService _keywordService;

        private KeywordRequestDto validDto;
        private KeywordRequestDto invalidDto;
        private RandomKeywordRequestDto validRandomKeywordDto;
        private RandomKeywordRequestDto invalidRandomKeywordDto;
        public KeywordServiceTests()
        {
            _repositoryMock = new Mock<IKeywordRepository>();
            _validationProviderMock = new Mock<IValidationProvider>();
            _validatorFactory = new Mock<IValidatorFactory>();
            _languageRepositoryMock = new Mock<ILanguageRepository>();

            _validatorFactory
                .Setup(s => s.GetValidator<KeywordRequestDto>())
                .Returns(new KeywordRequestDtoValidator());

            _repositoryMock
                .Setup(s => s.CreateAsync<KeywordRequestDto, KeywordResponseDto>(It.IsAny<KeywordRequestDto>()))
                .ReturnsAsync(new KeywordResponseDto());
            _repositoryMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _repositoryMock
                .Setup(s => s.GetByIdAsync<KeywordResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new KeywordResponseDto());
            _repositoryMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<KeywordRequestDto>()));

            _languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _keywordService = new KeywordService(_repositoryMock.Object,_languageRepositoryMock.Object,_validationProviderMock.Object);

            invalidDto = new KeywordRequestDto()
            {
                Word = "",
                LanguageId = Guid.NewGuid()
            };
            validDto = new KeywordRequestDto()
            {
                Word = "Test",
                LanguageId = Guid.NewGuid()
            };
            invalidRandomKeywordDto = new RandomKeywordRequestDto()
            {
                Difficulties = new List<Difficulty> { },
                LanguageId = Guid.NewGuid()
            };
            validRandomKeywordDto = new RandomKeywordRequestDto()
            {
                Difficulties = new List<Difficulty> { Difficulty.Medium, Difficulty.Hard },
                LanguageId = Guid.NewGuid()
            };
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNewResponseDto()
        {
            // Arrange
            var expected = new KeywordResponseDto();

            // Act
            var result = await _keywordService.CreateAsync(validDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repositoryMock.Verify(r => r.CreateAsync<KeywordRequestDto,KeywordResponseDto>(validDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            _validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<KeywordRequestDto>()))
                .Throws(new ValidationException("test"));

            // Act
            async Task test() => await _keywordService.CreateAsync(invalidDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task CreateAsync_InvalidLanguageGiven_ExpectedException()
        {
            // Arrange
            _languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException($"Language was not found!"));

            // Act
            async Task test() => await _keywordService.CreateAsync(invalidDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidLanguageIdGiven_ExpectedException()
        {
            // Arrange
            _languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException($"Language was not found!"));
            // Act
            async Task test() => await _keywordService.UpdateAsync(default, new KeywordRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            _validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<KeywordRequestDto>()))
                .Throws(new ValidationException("test"));
            // Act
            async Task test() => await _keywordService.UpdateAsync(Guid.NewGuid(), invalidDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingMethodOnce()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            await _keywordService.UpdateAsync(id, validDto);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(id, validDto), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _keywordService.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ExpectedException()
        {
            //Arrange
            _repositoryMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException($"{typeof(LanguageResponseDto).Name} was not found!"));
            // Act
            async Task test() => await _keywordService.DeleteAsync(default);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ExpectedException()
        {
            //Arrange
            _repositoryMock
                .Setup(s => s.GetByIdAsync<KeywordResponseDto>(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException($"{typeof(LanguageResponseDto).Name} was not found!"));

            // Act
            async Task test() => await _keywordService.GetByIdAsync(default);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidIdGiven_ExpectedNormalBehaviour()
        {
            // Arrange
            // Act
            var result = await _keywordService.GetByIdAsync(default);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(r => r.GetByIdAsync<KeywordResponseDto>(default), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var pagedResult = new PaginatedResult<KeywordResponseDto>(new List<KeywordResponseDto>(), 0, 0, 0);

            _repositoryMock
                .Setup(s => s.GetPageAsync<KeywordResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _keywordService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result);
            _repositoryMock.Verify(r => r.GetPageAsync<KeywordResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task GetRandomKeywordAsync_NotValid_ExpectedException()
        {
            //Arrange
            _validationProviderMock
             .Setup(s => s.TryValidate(invalidRandomKeywordDto))
             .Throws(new ValidationException("test"));

            _languageRepositoryMock
             .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
             .ReturnsAsync(true);

            // Act
            async Task test() => await _keywordService.GetRandomKeywordAsync(invalidRandomKeywordDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task GetRandomKeywordAsync_LanguageIdNotFound_ExpectedException()
        {
            //Arrange
            _validatorFactory
             .Setup(s => s.GetValidator<RandomKeywordRequestDto>())
             .Returns(new RandomKeywordRequestDtoValidator());

            _languageRepositoryMock
             .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
             .ThrowsAsync(new NotFoundException($"Language was not found!"));

            // Act
            async Task test() => await _keywordService.GetRandomKeywordAsync(validRandomKeywordDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetRandomKeywordAsync_ValidData_ExpectedNormalBehaviour()
        {
            //Arrange
            RandomKeywordResponseDto responseDto = new RandomKeywordResponseDto {
                KeywordId = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14"),
                Word = "int"
            };

            _validatorFactory
            .Setup(s => s.GetValidator<RandomKeywordRequestDto>())
            .Returns(new RandomKeywordRequestDtoValidator());

            _languageRepositoryMock
             .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
             .ReturnsAsync(true);

            _repositoryMock
             .Setup(s => s.GetRandomKeywordAsync(validRandomKeywordDto))
             .ReturnsAsync(responseDto);

            // Act
            var result = await _keywordService.GetRandomKeywordAsync(validRandomKeywordDto);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(r => r.GetRandomKeywordAsync(validRandomKeywordDto), Times.Once());
        }
    }
}
