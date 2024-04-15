using Moq;
using Prime.Progreso.Domain.Abstractions.Repositories;
using System.Linq.Expressions;
using Xunit;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Dtos.Projects;
using Prime.Progreso.Domain.Validators.Project;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services.Test
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> repositoryMock;
        private readonly Mock<IMilestoneRepository> milestoneRepositoryMock;
        private readonly Mock<IValidatorFactory> validatorFactory;
        private readonly IValidationProvider validationProvider;

        public ProjectServiceTests()
        {
            repositoryMock = new Mock<IProjectRepository>();
            milestoneRepositoryMock = new Mock<IMilestoneRepository>();
            validatorFactory = new Mock<IValidatorFactory>();
            validationProvider = new ValidationProvider(validatorFactory.Object);
        }

        private void CompareProperties(ProjectResponseDto responseDto, ProjectResponseDto actual)
        {
            Assert.Equal(responseDto.Id, actual.Id);
            Assert.Equal(responseDto.Title, actual.Title);
            Assert.Equal(responseDto.Description, actual.Description);
            Assert.Equal(responseDto.Milestones, actual.Milestones);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreateProject_ReturnsTheNewProject()
        {
            ProjectRequestDto requestDto = new ProjectRequestDto()
            {
                Title = "Project1",
                Description = "description",
                Milestones = new List<Guid>() { Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14") }
            };

            ProjectResponseDto responseDto = new ProjectResponseDto()
            {
                Id = Guid.Parse("4F996D4C-FB60-4510-B523-F6151E7DED14"),
                Title = "Project1",
                Description = "description",
                Milestones = new List<Guid>() { Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14") }
            };

            ProjectRequestDtoValidator validator = new ProjectRequestDtoValidator();

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            validatorFactory.Setup(s => s.GetValidator<ProjectRequestDto>())
                .Returns(validator);

            milestoneRepositoryMock.Setup(s => s.DoAllMilestonesExist(requestDto.Milestones))
                .Returns(true);

            repositoryMock.Setup(s => s.CreateAsync(requestDto))
                .ReturnsAsync(responseDto);

            var actual = await service.CreateAsync(requestDto);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.CreateAsync(requestDto), Times.Once());
        } 
        
        [Fact]
        public async Task CreateAsync_NotAllMilestonesExist_ThrowsNotFoundException()
        {
            ProjectRequestDto requestDto = new ProjectRequestDto()
            {
                Title = "Project1",
                Description = "description",
                Milestones = new List<Guid>() { Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14") }
            };

            ProjectRequestDtoValidator validator = new ProjectRequestDtoValidator();

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            validatorFactory.Setup(s => s.GetValidator<ProjectRequestDto>())
                .Returns(validator);

            milestoneRepositoryMock.Setup(s => s.DoAllMilestonesExist(requestDto.Milestones))
                .Returns(false);

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(requestDto));
        }

        [Fact]
        public async Task UpdateAsync_SuccessfullyUpdatedProject_ReturnsTheUpdatedProject()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            ProjectRequestDto requestDto = new ProjectRequestDto()
            {
                Title = "Project1",
                Description = "description",
                Milestones = new List<Guid>() { Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14") }
            };

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            ProjectRequestDtoValidator validator = new ProjectRequestDtoValidator();

            validatorFactory.Setup(s => s.GetValidator<ProjectRequestDto>())
             .Returns(validator);

            milestoneRepositoryMock.Setup(s => s.DoAllMilestonesExist(requestDto.Milestones))
             .Returns(true);

            repositoryMock.Setup(s => s.UpdateAsync(id, requestDto))
                .Returns(Task.CompletedTask);

            await service.UpdateAsync(id, requestDto);

            repositoryMock.Verify(x => x.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NotAllMilestonesExist_ThrowsNotFoundException()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            ProjectRequestDto requestDto = new ProjectRequestDto()
            {
                Title = "Project1",
                Description = "description",
                Milestones = new List<Guid>() { Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14") }
            };

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            ProjectRequestDtoValidator validator = new ProjectRequestDtoValidator();

            validatorFactory.Setup(s => s.GetValidator<ProjectRequestDto>())
             .Returns(validator);

            milestoneRepositoryMock.Setup(s => s.DoAllMilestonesExist(requestDto.Milestones))
             .Returns(false);

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(id, requestDto));
        }

        [Fact]
        public async Task DeleteAsync_SuccessfullyDeleteProject_VerifyThatDeleteWasInvockedOnce()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.DeleteAsync(id))
             .Returns(Task.CompletedTask);

            await service.DeleteAsync(id);

            repositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_SuccessfullyGetProjectById_ReturnASpecificProjectById()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            ProjectResponseDto responseDto = new ProjectResponseDto();

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.GetByIdAsync(id))
              .ReturnsAsync(responseDto);

            var actual = await service.GetByIdAsync(id);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_SuccessfullyGetAllProjects_ReturnNumberOfProjectsFromChoosedPage()
        {
            int pageNumber = 1;
            int pageSize = 10;
            int count = 10;

            List<ProjectResponseDto> responseDtos = new List<ProjectResponseDto>();

            Expression<Func<ProjectResponseDto, bool>> filter = null;

            PaginatedResult<ProjectResponseDto> paginatedResult = new PaginatedResult<ProjectResponseDto>(responseDtos, count, pageNumber, pageSize);

            var service = new ProjectService(repositoryMock.Object, milestoneRepositoryMock.Object, validationProvider);

            repositoryMock.Setup(s => s.GetPageAsync(pageNumber, pageSize, filter))
                .ReturnsAsync(paginatedResult);

            var actual = await service.GetPageAsync(pageNumber, pageSize);

            for (int i = 0; i < responseDtos.Count; i++)
            {
                Assert.Equal(responseDtos[i].Title, actual.Content[i].Title);
                Assert.Equal(responseDtos[i].Description, actual.Content[i].Description);
                Assert.Equal(responseDtos[i].Milestones, actual.Content[i].Milestones);
            }

            repositoryMock.Verify(r => r.GetPageAsync<ProjectResponseDto>(1, 10, null), Times.Once());
        }
    }
}
