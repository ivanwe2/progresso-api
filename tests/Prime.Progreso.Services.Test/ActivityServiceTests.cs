using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Enums;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.Activity;
using Xunit;
using IValidatorFactory = Prime.Progreso.Domain.Abstractions.Factories.IValidatorFactory;

namespace Prime.Progreso.Services.Test
{
    public class ActivityServiceTests
    {
        public IActivityService activityService;
        public Mock<IActivityRepository> activityRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public IValidationProvider validationProvider;

        public ActivityRequestDto validActivityRequestDto;

        public ActivityServiceTests() 
        {
            activityRepoMock = new Mock<IActivityRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();

            activityRepoMock
                .Setup(s => s.CreateAsync<ActivityRequestDto, ActivityResponseDto>(It.IsAny<ActivityRequestDto>()))
                .ReturnsAsync(new ActivityResponseDto());
            activityRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            activityRepoMock
                .Setup(s => s.GetByIdAsync<ActivityResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new ActivityResponseDto());
            activityRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ActivityRequestDto>()));

            validatorFactoryMock
                .Setup(s => s.GetValidator<ActivityRequestDto>())
                .Returns(new ActivityRequestDtoValidator());

            validationProvider = new ValidationProvider(validatorFactoryMock.Object);

            activityService = new ActivityService(activityRepoMock.Object,
                validationProvider);

            validActivityRequestDto = new ActivityRequestDto()
            {
                Subject = "subject",
                Description = "description",
                Type = ActivityType.Meeting
            };
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var expected = new ActivityResponseDto();

            // Act
            var result = await activityService.CreateAsync(validActivityRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            activityRepoMock.Verify(r => r.CreateAsync<ActivityRequestDto, ActivityResponseDto>(validActivityRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var activityRequestDto = new ActivityRequestDto()
            {
                Subject = "",
                Description = "description",
                Type = ActivityType.Meeting
            };

            // Act
            async Task a() => await activityService.CreateAsync(activityRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new ActivityResponseDto() { Id = id };

            activityRepoMock
                .Setup(s => s.GetByIdAsync<ActivityResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new ActivityResponseDto() { Id = id });

            activityService = new ActivityService(activityRepoMock.Object,
                validationProvider);

            // Act
            var result = await activityService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            activityRepoMock.Verify(r => r.GetByIdAsync<ActivityResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidActivityId_ExpectedException()
        {
            // Act
            async Task a() => await activityService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<ActivityResponseDto>(new List<ActivityResponseDto>(), 0, 0, 0);

            activityRepoMock
                .Setup(s => s.GetPageAsync<ActivityResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var activities = await activityService.GetPageAsync(1, 10, null);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            activityRepoMock.Verify(r => r.GetPageAsync<ActivityResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepositoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await activityService.UpdateAsync(id, validActivityRequestDto);

            // Assert
            activityRepoMock.Verify(r => r.UpdateAsync<ActivityRequestDto>(id, validActivityRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidActivityId_ExpectedException()
        {
            // Act
            async Task a() => await activityService.UpdateAsync(default(Guid), new ActivityRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await activityService.DeleteAsync(id);

            // Assert
            activityRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidActivityId_ExpectedException()
        {
            // Act
            async Task a() => await activityService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
