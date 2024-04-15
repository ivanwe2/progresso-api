using Moq;
using Prime.Progreso.Domain.Abstractions.Repositories;
using System.Linq.Expressions;
using Xunit;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Validators.KeywordDescription;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services.Test
{
    public class KeywordDescriptionServiceTests
    {
        private readonly Mock<IKeywordDescriptionRepository> repositoryMock;
        private readonly Mock<IValidatorFactory> validatorFactory;
        private readonly Mock<IValidationProvider> validationProviderMock;
        private readonly Mock<IKeywordRepository> keywordRepositoryMock;
        private readonly Mock<ILanguageRepository> languageRepositoryMock;

        public KeywordDescriptionServiceTests()
        {
            repositoryMock = new Mock<IKeywordDescriptionRepository>();
            validatorFactory = new Mock<IValidatorFactory>();
            keywordRepositoryMock = new Mock<IKeywordRepository>();
            languageRepositoryMock = new Mock<ILanguageRepository>();

            keywordRepositoryMock.Setup(x => x.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            languageRepositoryMock
                .Setup(x => x.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            validatorFactory
                .Setup(s => s.GetValidator<KeywordDescriptionRequestDto>())
                .Returns(new KeywordDescriptionRequestDtoValidator());

            validationProviderMock = new Mock<IValidationProvider>();
        }

        private void CompareProperties(KeywordDescriptionResponseDto responseDto, KeywordDescriptionResponseDto actual)
        {
            Assert.Equal(responseDto.Id, actual.Id);
            Assert.Equal(responseDto.Description, actual.Description);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreateKeywordDescription_ReturnsTheNewKeywordDescription()
        {
            KeywordDescriptionRequestDto requestDto = new KeywordDescriptionRequestDto()
            {
                Description = "description",
            };

            KeywordDescriptionResponseDto responseDto = new KeywordDescriptionResponseDto()
            {
                Id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14"),
                Description = "description",
            };

            KeywordDescriptionRequestDtoValidator validator = new KeywordDescriptionRequestDtoValidator();

            var service = new KeywordDescriptionService(repositoryMock.Object, keywordRepositoryMock.Object,
                languageRepositoryMock.Object, validationProviderMock.Object);

            validatorFactory.Setup(s => s.GetValidator<KeywordDescriptionRequestDto>())
                .Returns(validator);

            repositoryMock.Setup(s => s.CreateAsync<KeywordDescriptionRequestDto, KeywordDescriptionResponseDto>(requestDto))
                .ReturnsAsync(responseDto);

            var actual = await service.CreateAsync(requestDto);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.CreateAsync<KeywordDescriptionRequestDto, KeywordDescriptionResponseDto>(requestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_SuccessfullyUpdatedKeywordDescription_ReturnsTheUpdatedKeywordDescription()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            KeywordDescriptionRequestDto requestDto = new KeywordDescriptionRequestDto()
            {
                Description = "description",
            };

            var service = new KeywordDescriptionService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            KeywordDescriptionRequestDtoValidator validator = new KeywordDescriptionRequestDtoValidator();

            validatorFactory.Setup(s => s.GetValidator<KeywordDescriptionRequestDto>())
             .Returns(validator);

            repositoryMock.Setup(s => s.UpdateAsync(id, requestDto))
                .Returns(Task.CompletedTask);

            await service.UpdateAsync(id, requestDto);

            repositoryMock.Verify(x => x.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfullyDeleteKeywordDescription_VerifyThatDeleteWasInvockedOnce()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            var service = new KeywordDescriptionService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            repositoryMock.Setup(s => s.DeleteAsync(id))
             .Returns(Task.CompletedTask);

            await service.DeleteAsync(id);

            repositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_SuccessfullyGetKeywordDescriptionById_ReturnASpecificKeywordDescriptionById()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            KeywordDescriptionResponseDto responseDto = new KeywordDescriptionResponseDto();

            var service = new KeywordDescriptionService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            repositoryMock.Setup(s => s.GetByIdAsync<KeywordDescriptionResponseDto>(id))
              .ReturnsAsync(responseDto);

            var actual = await service.GetByIdAsync(id);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.GetByIdAsync<KeywordDescriptionResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_SuccessfullyGetAllKeywordDescriptions_ReturnNumberOfKeywordDescriptionsFromChoosedPage()
        {
            int pageNumber = 1;
            int pageSize = 10;
            int count = 10;

            List<KeywordDescriptionResponseDto> responseDtos = new List<KeywordDescriptionResponseDto>();

            Expression<Func<KeywordDescriptionResponseDto, bool>> filter = null;

            PaginatedResult<KeywordDescriptionResponseDto> paginatedResult = new PaginatedResult<KeywordDescriptionResponseDto>(responseDtos, count, pageNumber, pageSize);

            var service = new KeywordDescriptionService(repositoryMock.Object, keywordRepositoryMock.Object, 
                languageRepositoryMock.Object, validationProviderMock.Object);

            repositoryMock.Setup(s => s.GetPageAsync(pageNumber, pageSize, filter))
                .ReturnsAsync(paginatedResult);

            var actual = await service.GetPageAsync(pageNumber, pageSize);

            for (int i = 0; i < responseDtos.Count; i++)
            {
                Assert.Equal(responseDtos[i].Description, actual.Content[i].Description);
            }

            repositoryMock.Verify(r => r.GetPageAsync<KeywordDescriptionResponseDto>(1, 10, null), Times.Once());
        }

    }
}
