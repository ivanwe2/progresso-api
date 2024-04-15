using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.TestCase;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class TestCaseServiceTests
    {
        public ITestCaseService testCaseService;
        public Mock<ITestCaseRepository> testCaseRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<ICodingChallengeRepository> codingChallengeRepoMock;
        public Mock<IAssignmentToChallengeRepository> assignmentRepoMock;
        public Mock<IUserDetailsProvider> userDetailsProviderMock;

        public TestCaseServiceTests()
        {
            testCaseRepoMock = new Mock<ITestCaseRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            codingChallengeRepoMock = new Mock<ICodingChallengeRepository>();
            assignmentRepoMock = new Mock<IAssignmentToChallengeRepository>();
            userDetailsProviderMock = new();

            testCaseRepoMock
                .Setup(s => s.CreateAsync<TestCaseRequestDto, TestCaseResponseDto>(It.IsAny<TestCaseRequestDto>()))
                .ReturnsAsync(new TestCaseResponseDto());
            testCaseRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            testCaseRepoMock
                .Setup(s => s.GetByIdAsync<TestCaseResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestCaseResponseDto());
            testCaseRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TestCaseRequestDto>()));

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<TestCaseRequestDto>()));

            codingChallengeRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            testCaseService = new TestCaseService(testCaseRepoMock.Object,
                                                  validationProviderMock.Object,
                                                  codingChallengeRepoMock.Object,
                                                  assignmentRepoMock.Object,
                                                  userDetailsProviderMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var testCaseRequestDto = new TestCaseRequestDto();

            var expected = new TestCaseResponseDto();

            // Act
            var result = await testCaseService.CreateAsync(testCaseRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            testCaseRepoMock.Verify(r =>
                r.CreateAsync<TestCaseRequestDto, TestCaseResponseDto>(testCaseRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var testCaseRequestDto = new TestCaseRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<TestCaseRequestDto>()))
                .Throws(new ValidationException("sth"));

            // Act
            async Task a() => await testCaseService.CreateAsync(testCaseRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataWithInternRole_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new TestCaseResponseDto() { Id = id };
            var role = "ROLE_INTERN";
            var userId = 10;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = null
            };

            testCaseRepoMock
                .Setup(s => s.GetByIdAsync<TestCaseResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestCaseResponseDto() { Id = id });

            assignmentRepoMock
                .Setup(r => r.HasAnyAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            assignmentRepoMock
                .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(assignment);

            // Act
            var result = await testCaseService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            testCaseRepoMock.Verify(r => r.GetByIdAsync<TestCaseResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new TestCaseResponseDto() { Id = id };
            var role = "ROLE_ADMIN";
            var userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            testCaseRepoMock
                .Setup(s => s.GetByIdAsync<TestCaseResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestCaseResponseDto() { Id = id });

            // Act
            var result = await testCaseService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            testCaseRepoMock.Verify(r => r.GetByIdAsync<TestCaseResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_AssignmentStartTimeAfterCurrentTimeWithInternRole_ExpectedException()
        {
            // Arrange
            var role = "ROLE_INTERN";
            var userId = 1;
            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(+5),
                EndTime = null
            };

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock
               .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
               .ReturnsAsync(assignment);

            // Act
            async Task a() => await testCaseService.GetByIdAsync(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_AssignmentEndTimeBeforeCurrentTimeWithInternRole_ExpectedException()
        {
            // Arrange
            var role = "ROLE_INTERN";
            var userId = 1;
            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(-5),
                EndTime = DateTime.Now.AddDays(-4)
            };

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock
               .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
               .ReturnsAsync(assignment);

            // Act
            async Task a() => await testCaseService.GetByIdAsync(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidTestCaseId_ExpectedException()
        {
            // Act
            async Task a() => await testCaseService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<TestCaseResponseDto>(new List<TestCaseResponseDto>(), 0, 0, 0);
            var role = "ROLE_ADMIN";
            var userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var pagingInfo = new TestCasesPagingInfo
            {
                Page = 1,
                Size = 10
            };

            testCaseRepoMock
                .Setup(s => s.GetPageAsync<TestCaseResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var testCases = await testCaseService.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(testCases);
            Assert.Empty(testCases.Content);
            testCaseRepoMock.Verify(r => r.GetPageAsync<TestCaseResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersWithInternRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<TestCaseResponseDto>(new List<TestCaseResponseDto>(), 0, 0, 0);
            var codingChallengeIds = new List<Guid> { Guid.NewGuid() };

            var role = "ROLE_INTERN";
            var userId = 10;

            var pagingInfo = new TestCasesPagingInfo
            {
                Page = 1,
                Size = 10,
                codingChallengeIds = new List<Guid>()
            };

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            assignmentRepoMock
                .Setup(s => s.GetAssignedCodingChallengeIdsAsync(It.IsAny<int>()))
                .ReturnsAsync(codingChallengeIds);

            testCaseRepoMock
                .Setup(s => s.GetPageAsync(It.IsAny<TestCasesPagingInfo>()))
                .ReturnsAsync(page);

            // Act
            var testCases = await testCaseService.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(testCases);
            Assert.Empty(testCases.Content);
            testCaseRepoMock.Verify(r => r.GetPageAsync(pagingInfo), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersThereAreNoAssignedCodingChallengesWithInternRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<TestCaseResponseDto>(new List<TestCaseResponseDto>(), 0, 0, 0);
            var emptyCodingChallengeIds = new List<Guid>();

            var role = "ROLE_INTERN";
            var userId = 10;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var pagingInfo = new TestCasesPagingInfo
            {
                Page = 1,
                Size = 10,
                codingChallengeIds = new List<Guid>()
            };

            assignmentRepoMock
                .Setup(s => s.GetAssignedCodingChallengeIdsAsync(It.IsAny<int>()))
                .ReturnsAsync(emptyCodingChallengeIds);

            testCaseRepoMock
                .Setup(s => s.GetPageAsync(It.IsAny<TestCasesPagingInfo>()))
                .ReturnsAsync(page);

            // Act
            var testCases = await testCaseService.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(testCases);
            Assert.Empty(testCases.Content);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var testCaseRequestDto = new TestCaseRequestDto();

            // Act
            await testCaseService.UpdateAsync(id, testCaseRequestDto);

            // Assert
            testCaseRepoMock.Verify(r => r.UpdateAsync<TestCaseRequestDto>(id, testCaseRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidTestCaseId_ExpectedException()
        {
            // Act
            async Task a() => await testCaseService.UpdateAsync(default(Guid), new TestCaseRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await testCaseService.DeleteAsync(id);

            // Assert
            testCaseRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidTestCaseId_ExpectedException()
        {
            // Act
            async Task a() => await testCaseService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
