using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators.AssignmentToChallenge;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class AssignmentToChallengeServiceTests
    {
        public IAssignmentToChallengeService assignmentService;
        public Mock<IAssignmentToChallengeRepository> assignmentRepoMock;
        public Mock<ICodingChallengeRepository> codingChallengeRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<IUserDetailsProvider> userDetailsProviderMock;

        public AssignmentToChallengeServiceTests()
        {
            assignmentRepoMock = new Mock<IAssignmentToChallengeRepository>();
            codingChallengeRepoMock = new Mock<ICodingChallengeRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();
            validationProviderMock = new Mock<IValidationProvider>();
            userDetailsProviderMock = new();

            assignmentRepoMock
                .Setup(r => r.AddOrUpdateAsync(It.IsAny<AssignmentRequestDto>()))
                .ReturnsAsync(new AssignmentResponseDto());
            assignmentRepoMock.Setup(r => r.UnassignInternAsync(It.IsAny<UnassignmentRequestDto>()));
            assignmentRepoMock.Setup(r => r.GetByIdAsync<AssignmentResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new AssignmentResponseDto());
            assignmentRepoMock.Setup(r => r.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(new AssignmentResponseDto());
            assignmentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(new AssignmentResponseDto());
            assignmentRepoMock
                .Setup(r => r.GetPageAndFilterByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PaginatedResult<AssignmentResponseDto>(new List<AssignmentResponseDto>(), 0, 0, 0));
            assignmentRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>()));
            //assignmentRepoMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(false);

            codingChallengeRepoMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            validatorFactoryMock
                .Setup(s => s.GetValidator<AssignmentRequestDto>())
                .Returns(new AssignmentRequestDtoValidator());

            assignmentService = new AssignmentToChallengeService(assignmentRepoMock.Object,
                                                                 codingChallengeRepoMock.Object,
                                                                 validationProviderMock.Object,
                                                                 userDetailsProviderMock.Object);
        }
        
        [Fact]
        public async Task AssignInternAsync_ValidData_ExpectedNotNull()
        {
            //Arrange
            var validDto = new AssignmentRequestDto
            {
                InternId = 1,
                CodingChallengeId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow
            };

            var expectedDto = new AssignmentResponseDto();

            //Act
            var result = await assignmentService.AssignInternAsync(validDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equivalent(result, expectedDto);
            assignmentRepoMock.Verify(r => r.AddOrUpdateAsync(validDto), Times.Once());
        }

        [Fact]
        public async Task AssignInternAsync_InvalidCodingChallengeId_ExpectedException()
        {
            var dto = new AssignmentRequestDto();

            codingChallengeRepoMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            async Task a() => await assignmentService.AssignInternAsync(dto);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(a);
        }

        [Fact]
        public async Task AssignInternAsync_InvalidData_ExpectedException()
        {
            // Arrange

            var dto = new AssignmentRequestDto
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(-1)
            };

            validationProviderMock
                .Setup(x => x.TryValidateAsync(It.IsAny<AssignmentRequestDto>()))
                .ThrowsAsync(new ValidationException("End time must be after start time."));

            //Act
            async Task a() => await assignmentService.AssignInternAsync(dto);

            //Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await assignmentService.DeleteAsync(id);

            // Assert
            assignmentRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidAssignmentId_ExpectedException()
        {
            // Act
            async Task a() => await assignmentService.DeleteAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var role = "ROLE_ADMIN";
            var expected = new AssignmentResponseDto() { Id = id };

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);

            assignmentRepoMock.Setup(r => r.GetByIdAsync<AssignmentResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new AssignmentResponseDto() { Id = id });

            // Act
            var result = await assignmentService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            assignmentRepoMock.Verify(r => r.GetByIdAsync<AssignmentResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidAssignmentId_ExpectedException()
        {
            // Act
            async Task a() => await assignmentService.GetByIdAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNullUsingUserIdParameter()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = 1;
            var role = "ROLE_INTERN";
            var expected = new AssignmentResponseDto() { Id = id };

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), userId))
                .ReturnsAsync(new AssignmentResponseDto() { Id = id });

            // Act
            var result = await assignmentService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            assignmentRepoMock.Verify(r => r.GetByIdAsync(id, userId), Times.Once());
        }

        [Fact]
        public async Task GetPageAsync_ValidParameters_ExpectedEmptyCollectionWithUserIdFilter()
        {
            // Arrange
            var page = new PaginatedResult<AssignmentResponseDto>(new List<AssignmentResponseDto>(), 0, 0, 0);
            var role = "ROLE_INTERN";
            var userId = 2;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock
                .Setup(s => s.GetPageAndFilterByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(page);

            // Act
            var assignments = await assignmentService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(assignments);
            Assert.Empty(assignments.Content);
            assignmentRepoMock.Verify(r => r.GetPageAndFilterByUserIdAsync(1, 10, 2), Times.Once());
        }

        [Fact]
        public async Task GetPageAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<AssignmentResponseDto>(new List<AssignmentResponseDto>(), 0, 0, 0);
            var role = "ROLE_ADMIN";
            var userId = 0;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock
                .Setup(s => s.GetPageAsync<AssignmentResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var assignments = await assignmentService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(assignments);
            Assert.Empty(assignments.Content);
            assignmentRepoMock.Verify(r => r.GetPageAsync<AssignmentResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UnassignInternAsync_ValidData_ExpectedInvokingRepositoryUnassignAsyncMethodOnce()
        {
            //Arrange
            var existingDto = new AssignmentResponseDto
            {
                StartTime = DateTime.UtcNow
            };

            var requestDto = new UnassignmentRequestDto
            {
                InternId = 1,
                CodingChallengeId = Guid.NewGuid(),
                EndTime = DateTime.UtcNow
            };

            assignmentRepoMock
                .Setup(r => r.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingDto);

            //Act
            await assignmentService.UnassignInternAsync(requestDto);

            //Assert
            assignmentRepoMock.Verify(r => r.UnassignInternAsync(requestDto), Times.Once());
        }

        [Fact]
        public async Task UnassignInternAsync_InvalidCodingChallengeId_ExpectedException()
        {
            //Arrange
            var dto = new UnassignmentRequestDto();

            codingChallengeRepoMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            async Task a() => await assignmentService.UnassignInternAsync(dto);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(a);
        }

        [Fact]
        public async Task UnassignInternAsync_StartTimeIsGreaterThanEndTime_ExpectedException()
        {
            //Arrange
            var requestDto = new UnassignmentRequestDto
            {
                InternId = 1,
                CodingChallengeId = Guid.NewGuid(),
                EndTime = DateTime.UtcNow
            };

            var existingDto = new AssignmentResponseDto
            {
                StartTime = DateTime.UtcNow
            };

            assignmentRepoMock
                .Setup(r => r.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingDto);

            //Act
            async Task a() => await assignmentService.UnassignInternAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }
    }
}
