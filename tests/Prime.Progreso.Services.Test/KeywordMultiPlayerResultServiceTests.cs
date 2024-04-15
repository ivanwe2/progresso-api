using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Exceptions;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class KeywordMultiPlayerResultServiceTests
    {
        private IKeywordMultiPlayerResultService service;
        private readonly Mock<IKeywordMultiPlayerResultRepository> repositoryMock;
        private readonly Mock<IKeywordRepository> keywordRepositoryMock;
        private readonly Mock<IValidationProvider> validationProviderMock;

        public KeywordMultiPlayerResultServiceTests()
        {
            repositoryMock = new Mock<IKeywordMultiPlayerResultRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            keywordRepositoryMock = new Mock<IKeywordRepository>();

            repositoryMock
                .Setup(s => s.CreateAsync<KeywordMultiPlayerResultRequestDto, KeywordMultiPlayerResultResponseDto>(
                    It.IsAny<KeywordMultiPlayerResultRequestDto>()))
                .ReturnsAsync(new KeywordMultiPlayerResultResponseDto());

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<TestCaseRequestDto>()));

            keywordRepositoryMock
            .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            service = new KeywordMultiPlayerResultService(repositoryMock.Object, keywordRepositoryMock.Object,
                validationProviderMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var keywordMultiPlayerResultRequestDto = new KeywordMultiPlayerResultRequestDto();

            var expected = new KeywordMultiPlayerResultResponseDto();

            // Act
            var result = await service.CreateAsync(keywordMultiPlayerResultRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            repositoryMock.Verify(r => r
                .CreateAsync<KeywordMultiPlayerResultRequestDto, KeywordMultiPlayerResultResponseDto>(
                    It.IsAny<KeywordMultiPlayerResultRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var keywordMultiPlayerResultRequestDto = new KeywordMultiPlayerResultRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidateAsync(It.IsAny<KeywordMultiPlayerResultRequestDto>()))
                .ThrowsAsync(new ValidationException("sth"));

            service = new KeywordMultiPlayerResultService(
                repositoryMock.Object, keywordRepositoryMock.Object, validationProviderMock.Object);

            // Act
            async Task a() => await service.CreateAsync(keywordMultiPlayerResultRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }
    }
}
