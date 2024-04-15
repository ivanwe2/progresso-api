using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.CurriculumItem;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class CurriculumItemServiceTests
    {
        public ICurriculumItemService curriculumItemService;
        public Mock<ICurriculumItemRepository> curriculumItemRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public Mock<IActivityRepository> activityRepoMock;
        public Mock<ICurriculumRepository> curriculumRepoMock;
        public IValidationProvider validationProvider;

        public CurriculumItemRequestDto validCurriculumItemRequestDto;

        public CurriculumItemServiceTests()
        {
            curriculumItemRepoMock = new Mock<ICurriculumItemRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();
            activityRepoMock = new Mock<IActivityRepository>();
            curriculumRepoMock = new Mock<ICurriculumRepository>();

            curriculumItemRepoMock
                .Setup(s => s.CreateAsync<CurriculumItemRequestDto, CurriculumItemResponseDto>(
                    It.IsAny<CurriculumItemRequestDto>()))
                .ReturnsAsync(new CurriculumItemResponseDto());
            curriculumItemRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            curriculumItemRepoMock
                .Setup(s => s.GetByIdAsync<CurriculumItemResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new CurriculumItemResponseDto());
            curriculumItemRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CurriculumItemRequestDto>()));

            validatorFactoryMock
                .Setup(s => s.GetValidator<CurriculumItemRequestDto>())
                .Returns(new CurriculumItemRequestDtoValidator());

            validationProvider = new ValidationProvider(validatorFactoryMock.Object);

            activityRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            curriculumRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            curriculumItemService = new CurriculumItemService(curriculumItemRepoMock.Object,
                validationProvider, activityRepoMock.Object, curriculumRepoMock.Object);

            validCurriculumItemRequestDto = new CurriculumItemRequestDto()
            {
                ActivityId = Guid.NewGuid(),
                CurriculumId = Guid.NewGuid(),
                DayOfInternship = 1
            };
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var expected = new CurriculumItemResponseDto();

            // Act
            var result = await curriculumItemService.CreateAsync(validCurriculumItemRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            curriculumItemRepoMock.Verify(r => 
                r.CreateAsync<CurriculumItemRequestDto, CurriculumItemResponseDto>(validCurriculumItemRequestDto), 
                Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var curriculumItemRequestDto = new CurriculumItemRequestDto();

            // Act
            async Task a() => await curriculumItemService.CreateAsync(curriculumItemRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task CreateAsync_InvalidCurriculumId_ExpectedException()
        {
            // Arrange
            curriculumRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            curriculumItemService = new CurriculumItemService(curriculumItemRepoMock.Object,
                validationProvider, activityRepoMock.Object, curriculumRepoMock.Object);

            // Act
            async Task a() => await curriculumItemService.CreateAsync(validCurriculumItemRequestDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new CurriculumItemResponseDto() { Id = id };

            curriculumItemRepoMock
                .Setup(s => s.GetByIdAsync<CurriculumItemResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new CurriculumItemResponseDto() { Id = id });

            curriculumItemService = new CurriculumItemService(curriculumItemRepoMock.Object,
                validationProvider,  activityRepoMock.Object, curriculumRepoMock.Object);

            // Act
            var result = await curriculumItemService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            curriculumItemRepoMock.Verify(r => r.GetByIdAsync<CurriculumItemResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidActivityId_ExpectedException()
        {
            // Act
            async Task a() => await curriculumItemService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<CurriculumItemResponseDto>(new List<CurriculumItemResponseDto>(), 0, 0, 0);

            curriculumItemRepoMock
                .Setup(s => s.GetPageAsync<CurriculumItemResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var activities = await curriculumItemService.GetPageAsync(1, 10, null);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            curriculumItemRepoMock.Verify(r => r.GetPageAsync<CurriculumItemResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await curriculumItemService.UpdateAsync(id, validCurriculumItemRequestDto);

            // Assert
            curriculumItemRepoMock.Verify(r => 
                r.UpdateAsync<CurriculumItemRequestDto>(id, validCurriculumItemRequestDto), 
                Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidActivityId_ExpectedException()
        {
            // Arrange
            activityRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            curriculumItemService = new CurriculumItemService(curriculumItemRepoMock.Object,
                validationProvider, activityRepoMock.Object, curriculumRepoMock.Object);

            // Act
            async Task a() => await curriculumItemService.UpdateAsync(Guid.NewGuid(), validCurriculumItemRequestDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(a);
        }

        [Fact]
        public async Task UpdateAsync_InvalidCurriculumId_ExpectedException()
        {
            // Arrange
            curriculumRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            curriculumItemService = new CurriculumItemService(curriculumItemRepoMock.Object,
                validationProvider, activityRepoMock.Object, curriculumRepoMock.Object);

            // Act
            async Task a() => await curriculumItemService.UpdateAsync(Guid.NewGuid(), validCurriculumItemRequestDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await curriculumItemService.DeleteAsync(id);

            // Assert
            curriculumItemRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidActivityId_ExpectedException()
        {
            // Act
            async Task a() => await curriculumItemService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
