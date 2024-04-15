using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class KeywordDescriptionMultiPlayerResultServiceTests
    {
        public IKeywordDescriptionMultiPlayerResultService keywordDescriptionMultiPlayerResultService;
        public Mock<IKeywordDescriptionMultiPlayerResultRepository> keywordDescriptionMultiPlayerResultRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<IKeywordDescriptionRepository> keywordDescriptionRepoMock;

        public KeywordDescriptionMultiPlayerResultServiceTests()
        {
            keywordDescriptionMultiPlayerResultRepoMock = new Mock<IKeywordDescriptionMultiPlayerResultRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            keywordDescriptionRepoMock = new Mock<IKeywordDescriptionRepository>();

            keywordDescriptionMultiPlayerResultRepoMock
                .Setup(s => s.CreateAsync<KeywordDescriptionMultiPlayerResultRequestDto, KeywordDescriptionMultiPlayerResultResponseDto>(
                    It.IsAny<KeywordDescriptionMultiPlayerResultRequestDto>()))
                .ReturnsAsync(new KeywordDescriptionMultiPlayerResultResponseDto());

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<KeywordDescriptionSinglePlayerResultRequestDto>()));

            keywordDescriptionRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            keywordDescriptionMultiPlayerResultService = new KeywordDescriptionMultiPlayerResultService(
                keywordDescriptionMultiPlayerResultRepoMock.Object,
                validationProviderMock.Object, keywordDescriptionRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var keywordDescriptionMultiPlayerResultRequestDto = new KeywordDescriptionMultiPlayerResultRequestDto();

            var expected = new KeywordDescriptionMultiPlayerResultResponseDto();

            // Act
            var result = await keywordDescriptionMultiPlayerResultService.CreateAsync(keywordDescriptionMultiPlayerResultRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            keywordDescriptionMultiPlayerResultRepoMock.Verify(r => r
                .CreateAsync<KeywordDescriptionMultiPlayerResultRequestDto, KeywordDescriptionMultiPlayerResultResponseDto>(
                It.IsAny<KeywordDescriptionMultiPlayerResultRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var keywordDescriptionMultiPlayerResultRequestDto = new KeywordDescriptionMultiPlayerResultRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidateAsync(It.IsAny<KeywordDescriptionMultiPlayerResultRequestDto>()))
                .ThrowsAsync(new ValidationException("sth"));

            keywordDescriptionMultiPlayerResultService = new KeywordDescriptionMultiPlayerResultService(
                keywordDescriptionMultiPlayerResultRepoMock.Object,
                validationProviderMock.Object, keywordDescriptionRepoMock.Object);

            // Act
            async Task a() => await keywordDescriptionMultiPlayerResultService.CreateAsync(
                keywordDescriptionMultiPlayerResultRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }
    }
}
