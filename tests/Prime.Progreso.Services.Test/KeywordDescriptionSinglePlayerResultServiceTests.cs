using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Pagination;
using Xunit;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;
using AutoMapper;

namespace Prime.Progreso.Services.Test
{
    public class KeywordDescriptionSinglePlayerResultServiceTests
    {
        public IKeywordDescriptionSinglePlayerResultService keywordDescriptionSinglePlayerResultService;
        public Mock<IKeywordDescriptionSinglePlayerResultRepository> keywordDescriptionSinglePlayerResultRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<IKeywordDescriptionRepository> keywordDescriptionRepoMock;
        public Mock<IMapper> mapperMock;

        public KeywordDescriptionSinglePlayerResultServiceTests()
        {
            keywordDescriptionSinglePlayerResultRepoMock = new Mock<IKeywordDescriptionSinglePlayerResultRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            keywordDescriptionRepoMock = new Mock<IKeywordDescriptionRepository>();
            mapperMock = new Mock<IMapper>();

            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.CreateAsync(It.IsAny<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>()))
                .ReturnsAsync(new KeywordDescriptionSinglePlayerResultResponseDto());
            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.GetByIdAsync<KeywordDescriptionSinglePlayerResultResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new KeywordDescriptionSinglePlayerResultResponseDto());
            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), 
                It.IsAny<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>()));

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<KeywordDescriptionSinglePlayerResultRequestDto>()));

            keywordDescriptionRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            keywordDescriptionRepoMock
                .Setup(s => s.CheckIfAnswerIsCorrect(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            mapperMock
                .Setup(x => x.Map<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>(
                    It.IsAny<KeywordDescriptionSinglePlayerResultRequestDto>()))
                .Returns(new KeywordDescriptionSinglePlayerResultWithIsCorrectDto());

            keywordDescriptionSinglePlayerResultService = new KeywordDescriptionSinglePlayerResultService(
                keywordDescriptionSinglePlayerResultRepoMock.Object,
                validationProviderMock.Object, keywordDescriptionRepoMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var keywordDescriptionSinglePlayerResultRequestDto = new KeywordDescriptionSinglePlayerResultRequestDto();

            var expected = new KeywordDescriptionSinglePlayerResultResponseDto();

            // Act
            var result = await keywordDescriptionSinglePlayerResultService.CreateAsync(keywordDescriptionSinglePlayerResultRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            keywordDescriptionSinglePlayerResultRepoMock.Verify(r => r
                .CreateAsync(It.IsAny<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var keywordDescriptionSinglePlayerResultRequestDto = new KeywordDescriptionSinglePlayerResultRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidateAsync(It.IsAny<KeywordDescriptionSinglePlayerResultRequestDto>()))
                .ThrowsAsync(new ValidationException("sth"));

            keywordDescriptionSinglePlayerResultService = new KeywordDescriptionSinglePlayerResultService(
                keywordDescriptionSinglePlayerResultRepoMock.Object,
                validationProviderMock.Object, keywordDescriptionRepoMock.Object, mapperMock.Object);

            // Act
            async Task a() => await keywordDescriptionSinglePlayerResultService.CreateAsync(
                keywordDescriptionSinglePlayerResultRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new KeywordDescriptionSinglePlayerResultResponseDto() { Id = id };

            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.GetByIdAsync<KeywordDescriptionSinglePlayerResultResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new KeywordDescriptionSinglePlayerResultResponseDto() { Id = id });

            keywordDescriptionSinglePlayerResultService = new KeywordDescriptionSinglePlayerResultService(
                keywordDescriptionSinglePlayerResultRepoMock.Object,
                validationProviderMock.Object, keywordDescriptionRepoMock.Object, mapperMock.Object);

            // Act
            var result = await keywordDescriptionSinglePlayerResultService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            keywordDescriptionSinglePlayerResultRepoMock.Verify(r => r
                .GetByIdAsync<KeywordDescriptionSinglePlayerResultResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidKeywordDescriptionSinglePlayerResultId_ExpectedException()
        {
            // Act
            async Task a() => await keywordDescriptionSinglePlayerResultService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<KeywordDescriptionSinglePlayerResultResponseDto>(
                new List<KeywordDescriptionSinglePlayerResultResponseDto>(), 0, 0, 0);

            keywordDescriptionSinglePlayerResultRepoMock
                .Setup(s => s.GetPageAsync<KeywordDescriptionSinglePlayerResultResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var keywordDescriptionSinglePlayerResults = await keywordDescriptionSinglePlayerResultService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(keywordDescriptionSinglePlayerResults);
            Assert.Empty(keywordDescriptionSinglePlayerResults.Content);
            keywordDescriptionSinglePlayerResultRepoMock.Verify(r => r.GetPageAsync<KeywordDescriptionSinglePlayerResultResponseDto>
                (1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var keywordDescriptionSinglePlayerResultRequestDto = new KeywordDescriptionSinglePlayerResultRequestDto();

            // Act
            await keywordDescriptionSinglePlayerResultService.UpdateAsync(id, keywordDescriptionSinglePlayerResultRequestDto);

            // Assert
            keywordDescriptionSinglePlayerResultRepoMock.Verify(r => r
                .UpdateAsync(It.IsAny<Guid>(), It.IsAny<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>()), 
                    Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidKeywordDescriptionSinglePlayerResultId_ExpectedException()
        {
            // Act
            async Task a() => await keywordDescriptionSinglePlayerResultService.UpdateAsync(
                default(Guid), new KeywordDescriptionSinglePlayerResultRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            keywordDescriptionRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            await keywordDescriptionSinglePlayerResultService.DeleteAsync(id);

            // Assert
            keywordDescriptionSinglePlayerResultRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidKeywordDescriptionSinglePlayerResultId_ExpectedException()
        {
            // Act
            async Task a() => await keywordDescriptionSinglePlayerResultService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}