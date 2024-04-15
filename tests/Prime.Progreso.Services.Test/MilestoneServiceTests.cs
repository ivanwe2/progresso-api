using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.Milestones;
using Xunit;
using System.Linq.Expressions;
using Prime.Progreso.Domain.Validators.Milestone;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services.Test
{
    public class MilestoneServiceTests
    {
        private readonly Mock<IMilestoneRepository> repositoryMock;
        private readonly Mock<IValidatorFactory> validatorFactory;
        private IValidationProvider validationProvider;

        public MilestoneServiceTests()
        {
            repositoryMock = new Mock<IMilestoneRepository>();
            validatorFactory = new Mock<IValidatorFactory>();
            validationProvider = new ValidationProvider(validatorFactory.Object);
        }

        private void CompareProperties(MilestoneResponseDto responseDto, MilestoneResponseDto actual)
        {
            Assert.Equal(responseDto.Id, actual.Id);
            Assert.Equal(responseDto.Order, actual.Order);
            Assert.Equal(responseDto.Description, actual.Description);
            Assert.Equal(responseDto.Duration, actual.Duration);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreateMilestone_ReturnsTheNewMilestone()
        {
            MilestoneRequestDto requestDto = new MilestoneRequestDto()
            {
               Order = 1,
               Description = "description",
               Duration = 30
            };

            MilestoneResponseDto responseDto = new MilestoneResponseDto()
            {
                Id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14"),
                Order = 1,
                Description = "description",
                Duration = 30
            };

            MilestoneRequestDtoValidator validator = new MilestoneRequestDtoValidator();

            var service = new MilestoneService(repositoryMock.Object, validationProvider);

            validatorFactory.Setup(s => s.GetValidator<MilestoneRequestDto>())
                .Returns(validator);

            repositoryMock.Setup(s => s.CreateAsync<MilestoneRequestDto, MilestoneResponseDto>(requestDto))
                .ReturnsAsync(responseDto);

            var actual = await service.CreateAsync(requestDto);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.CreateAsync<MilestoneRequestDto, MilestoneResponseDto>(requestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_SuccessfullyUpdatedMilestone_ReturnsTheUpdatedMilestone()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            MilestoneRequestDto requestDto = new MilestoneRequestDto()
            {
                Order = 1,
                Description = "description",
                Duration = 30
            };

            var service = new MilestoneService(repositoryMock.Object, validationProvider);

            MilestoneRequestDtoValidator validator = new MilestoneRequestDtoValidator();

            validatorFactory.Setup(s => s.GetValidator<MilestoneRequestDto>())
             .Returns(validator);

            repositoryMock.Setup(s => s.UpdateAsync(id, requestDto))
                .Returns(Task.CompletedTask);

            await service.UpdateAsync(id, requestDto);

            repositoryMock.Verify(x => x.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfullyDeleteMilestone_VerifyThatDeleteWasInvockedOnce()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            var service = new MilestoneService(repositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.DeleteAsync(id))
             .Returns(Task.CompletedTask);

            await service.DeleteAsync(id);

            repositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_SuccessfullyGetMilestoneById_ReturnASpecificMilestoneById()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            MilestoneResponseDto responseDto = new MilestoneResponseDto();

            var service = new MilestoneService(repositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.GetByIdAsync<MilestoneResponseDto>(id))
              .ReturnsAsync(responseDto);

            var actual = await service.GetByIdAsync(id);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.GetByIdAsync<MilestoneResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_SuccessfullyGetAllMilestones_ReturnNumberOfMilestonesFromChoosedPage()
        {
            int pageNumber = 1;
            int pageSize = 10;
            int count = 10;

            List<MilestoneResponseDto> responseDtos = new List<MilestoneResponseDto>();

            Expression<Func<MilestoneResponseDto, bool>> filter = null;

            PaginatedResult<MilestoneResponseDto> paginatedResult = new PaginatedResult<MilestoneResponseDto>(responseDtos, count, pageNumber, pageSize);

            var service = new MilestoneService(repositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.GetPageAsync(pageNumber, pageSize, filter))
                .ReturnsAsync(paginatedResult);

            var actual = await service.GetPageAsync(pageNumber, pageSize);
            
            for (int i = 0; i < responseDtos.Count; i++)
            {
                Assert.Equal(responseDtos[i].Order, actual.Content[i].Order);
                Assert.Equal(responseDtos[i].Description, actual.Content[i].Description);
                Assert.Equal(responseDtos[i].Duration, actual.Content[i].Duration);
            }

            repositoryMock.Verify(r => r.GetPageAsync<MilestoneResponseDto>(1, 10, null), Times.Once());
        }

    }
}
