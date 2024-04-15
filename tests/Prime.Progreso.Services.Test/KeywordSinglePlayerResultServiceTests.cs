using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;
using Prime.Progreso.Domain.Validators.KeywordSinglePlayerResult;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class KeywordSinglePlayerResultServiceTests
    {
        private readonly Mock<IKeywordSinglePlayerResultRepository> repositoryMock;
        private readonly Mock<IKeywordRepository> keywordRepositoryMock;
        private readonly Mock<ILanguageRepository> languageRepositoryMock;
        private readonly Mock<IValidatorFactory> validatorFactory;
        private readonly Mock<IValidationProvider> validationProviderMock;

        public KeywordSinglePlayerResultServiceTests()
        {
            repositoryMock = new Mock<IKeywordSinglePlayerResultRepository>();
            keywordRepositoryMock = new Mock<IKeywordRepository>();
            languageRepositoryMock = new Mock<ILanguageRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            validatorFactory = new Mock<IValidatorFactory>();
        }

        private void CompareProperties(KeywordSinglePlayerResultResponseDto responseDto, KeywordSinglePlayerResultResponseDto actual)
        {
            Assert.Equal(responseDto.Id, actual.Id);
            Assert.Equal(responseDto.UserId, actual.UserId);
            Assert.Equal(responseDto.Answer, actual.Answer);
            Assert.Equal(responseDto.KeywordId, actual.KeywordId);
            Assert.Equal(responseDto.IsCorrect, actual.IsCorrect);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreateKeywordSinglePlayerResult_ReturnsTheNewKeywordSinglePlayerResult()
        {
            KeywordSinglePlayerResultRequestDto requestDto = new KeywordSinglePlayerResultRequestDto()
            {
                UserId = 1,
                Answer = "Test",
                KeywordId = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14")
            };

            KeywordSinglePlayerResultResponseDto responseDto = new KeywordSinglePlayerResultResponseDto()
            {
                Id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14"),
                UserId = 1,
                Answer = "Test",
                KeywordId = Guid.Parse("5D296D4C-FB60-4510-B523-F6151E7DED14"),
                IsCorrect = null
            };

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            validatorFactory
             .Setup(s => s.GetValidator<KeywordSinglePlayerResultRequestDto>())
             .Returns(new KeywordSinglePlayerResultRequestDtoValidator());

            keywordRepositoryMock
             .Setup(s => s.HasAnyAsync(requestDto.KeywordId))
             .ReturnsAsync(true);
            languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            repositoryMock
             .Setup(s => s.CreateAsync<KeywordSinglePlayerResultRequestDto, KeywordSinglePlayerResultResponseDto>(requestDto))
             .ReturnsAsync(responseDto);

            var actual = await service.CreateAsync(requestDto);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.CreateAsync<KeywordSinglePlayerResultRequestDto, KeywordSinglePlayerResultResponseDto>(requestDto),
                                  Times.Once());
        }

        [Fact]
        public async Task CreateAsync_KeywordIdDoesNotExist_ThrowsNotFoundException()
        {
            KeywordSinglePlayerResultRequestDto requestDto = new KeywordSinglePlayerResultRequestDto()
            {
                UserId = 1,
                Answer = "Test",
                KeywordId = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14")
            };

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            validatorFactory
            .Setup(s => s.GetValidator<KeywordSinglePlayerResultRequestDto>())
            .Returns(new KeywordSinglePlayerResultRequestDtoValidator());

            keywordRepositoryMock
                .Setup(s => s.HasAnyAsync(requestDto.KeywordId))
                .ReturnsAsync(false);
            languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(requestDto));
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedValidationException()
        {
            KeywordSinglePlayerResultRequestDto requestDto = new KeywordSinglePlayerResultRequestDto()
            {
                UserId = 1,
                Answer = "",
                KeywordId = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14")
            };

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);
            
            validationProviderMock
                .Setup(s => s.TryValidateAsync(requestDto))
                .Throws(new ValidationException("smth"));

            keywordRepositoryMock
             .Setup(s => s.HasAnyAsync(requestDto.KeywordId))
             .ReturnsAsync(true);
            languageRepositoryMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(requestDto));
        }

        [Fact]
        public async Task UpdateIsCorrectAsync__ValidData_ExpectedInvokingMethodOnce()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var validDto = new KeywordSinglePlayerResultIsCorrectUpdateRequestDto();

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            // Act
            await service.UpdateIsCorrectAsync(id, validDto);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(id, validDto), Times.Once());
        }

        [Fact]
        public async Task UpdateIsCorrectAsync_IdIsEmpty_ExpectedArgumentNullException()
        {
            // Arrange
            Guid id = Guid.Empty;
            var requestDto = new KeywordSinglePlayerResultIsCorrectUpdateRequestDto();

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            // Act
            async Task a() => await service.UpdateIsCorrectAsync(id, requestDto);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task UpdateIsCorrectAsync_InvalidData_ExpectedValidationException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var requestDto = new KeywordSinglePlayerResultIsCorrectUpdateRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidateAsync(requestDto))
                .ThrowsAsync(new ValidationException("smth"));

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            // Act
            async Task a() => await service.UpdateIsCorrectAsync(id, requestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }
        
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<KeywordSinglePlayerResultResponseDto>(
                new List<KeywordSinglePlayerResultResponseDto>(), 0, 0, 0);

            repositoryMock
                .Setup(s => s.GetPageAsync(It.IsAny<KeywordSinglePlayerResultPagingInfo>()))
                .ReturnsAsync(page);

            keywordRepositoryMock
             .Setup(s => s.DoAllKeywordsExist(It.IsAny<List<Guid>>()))
             .ReturnsAsync(true);
            languageRepositoryMock
                .Setup(s => s.DoAllLanguagesExist(It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);

            var service = new KeywordSinglePlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            var pagingInfo = new KeywordSinglePlayerResultPagingInfo
            {
                Page = 1,
                Size = 10,
                UserIds = new List<int> { 1 },
                KeywordIds = new List<Guid> { Guid.NewGuid() },
                LanguageIds = new List<Guid> { Guid.NewGuid() }
            };

            // Act
            var keywordDescriptionSinglePlayerResults = await service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(keywordDescriptionSinglePlayerResults);
            Assert.Empty(keywordDescriptionSinglePlayerResults.Content);
            repositoryMock.Verify(r => r.GetPageAsync(pagingInfo), Times.Once());
        }
    }
}
