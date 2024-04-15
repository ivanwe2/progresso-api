using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators.QuizExecution;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class QuizExecutionServiceTests
    {
        public IQuizExecutionService quizExecutionService;
        public Mock<IQuizExecutionRepository> quizExecutionRepoMock;
        public Mock<IValidatorFactory> validatorFactoryMock;
        public Mock<IQuizRepository> quizRepoMock;
        public Mock<IQuizAssignmentRepository> quizAssignmentRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<IUserDetailsProvider> userDetailsProviderMock;

        public QuizExecutionServiceTests()
        {
            quizExecutionRepoMock = new Mock<IQuizExecutionRepository>();
            validatorFactoryMock = new Mock<IValidatorFactory>();
            quizRepoMock = new Mock<IQuizRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            quizAssignmentRepoMock = new Mock<IQuizAssignmentRepository>();
            userDetailsProviderMock = new Mock<IUserDetailsProvider>();

            quizExecutionRepoMock
                .Setup(s => s.CreateAsync<QuizExecutionRequestDto, QuizExecutionResponseDto>(
                    It.IsAny<QuizExecutionRequestDto>()))
                .ReturnsAsync(new QuizExecutionResponseDto());
            quizExecutionRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync<QuizExecutionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizExecutionResponseDto());
            quizExecutionRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<QuizExecutionRequestDto>()));

            validatorFactoryMock
                .Setup(s => s.GetValidator<QuizExecutionRequestDto>())
                .Returns(new QuizExecutionRequestDtoValidator());

            quizRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            quizAssignmentRepoMock
                .Setup(s => s.IsInternAssignedToQuizAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            quizExecutionService = new QuizExecutionService(quizExecutionRepoMock.Object,
                validationProviderMock.Object, quizRepoMock.Object, quizAssignmentRepoMock.Object, userDetailsProviderMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var quizExecutionRequestDto = new QuizExecutionRequestDto();

            var expected = new QuizExecutionResponseDto();

            // Act
            var result = await quizExecutionService.CreateAsync(quizExecutionRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            quizExecutionRepoMock.Verify(r => 
                r.CreateAsync<QuizExecutionRequestDto, QuizExecutionResponseDto>(quizExecutionRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedValidationException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<QuizExecutionRequestDto>()))
                .Throws(new ValidationException("sth"));

            quizExecutionService = new QuizExecutionService(null, validationProviderMock.Object, null,null, null);

            // Act
            async Task test() => await quizExecutionService.CreateAsync(new QuizExecutionRequestDto());

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task CreateAsync_InvalidQuizId_ExpectedNotFoundException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<QuizExecutionRequestDto>()));

            quizRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            quizExecutionService = new QuizExecutionService(null, validationProviderMock.Object, quizRepoMock.Object, null, null);

            // Act
            async Task test() => await quizExecutionService.CreateAsync(new QuizExecutionRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithIntern_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new QuizExecutionResponseDto() { Id = id };
            string role = "ROLE_INTERN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), userId))
                .ReturnsAsync(new QuizExecutionResponseDto() { Id = id });

            quizExecutionService = new QuizExecutionService(quizExecutionRepoMock.Object, validationProviderMock.Object, null, 
                null, userDetailsProviderMock.Object); 

            // Act
            var result = await quizExecutionService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            quizExecutionRepoMock.Verify(r => r.GetByIdAsync(id, userId), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithAdminOrMentor_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new QuizExecutionResponseDto() { Id = id };
            string role = "ROLE_ADMIN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            quizExecutionRepoMock
                .Setup(s => s.GetByIdAsync<QuizExecutionResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizExecutionResponseDto() { Id = id });

            quizExecutionService = new QuizExecutionService(quizExecutionRepoMock.Object, validationProviderMock.Object, 
                null, null, userDetailsProviderMock.Object);

            // Act
            var result = await quizExecutionService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            quizExecutionRepoMock.Verify(r => r.GetByIdAsync<QuizExecutionResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidQuizExecutionId_ExpectedException()
        {
            string role = "ROLE_INTERN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Act
            async Task test() => await quizExecutionService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithIntern_ExpectedEmptyCollection()
        {
            string role = "ROLE_INTERN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Arrange
            var page = new PaginatedResult<QuizExecutionResponseDto>(new List<QuizExecutionResponseDto>(), 0, 0, 0);

            quizExecutionRepoMock
                  .Setup(s => s.GetPageAndFilterByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                  .ReturnsAsync(page);

            // Act
            var quizExecutions = await quizExecutionService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(quizExecutions);
            Assert.Empty(quizExecutions.Content);
            quizExecutionRepoMock.Verify(r => r.GetPageAndFilterByUserIdAsync(1, 10, userId), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithAdminOrMentor_ExpectedEmptyCollection()
        {
            string role = "ROLE_ADMIN";
            int userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Arrange
            var page = new PaginatedResult<QuizExecutionResponseDto>(new List<QuizExecutionResponseDto>(), 0, 0, 0);

            quizExecutionRepoMock
                .Setup(s => s.GetPageAsync<QuizExecutionResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var quizExecutions = await quizExecutionService.GetPageAsync(1, 10);

            // Assert
            Assert.NotNull(quizExecutions);
            Assert.Empty(quizExecutions.Content);
            quizExecutionRepoMock.Verify(r => r.GetPageAsync<QuizExecutionResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quizExecutionRequestDto = new QuizExecutionRequestDto();

            // Act
            await quizExecutionService.UpdateAsync(id, quizExecutionRequestDto);

            // Assert
            quizExecutionRepoMock.Verify(r => r.UpdateAsync<QuizExecutionRequestDto>(id, quizExecutionRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidQuizExecutionId_ExpectedException()
        {
            // Act
            async Task test() => await quizExecutionService.UpdateAsync(default(Guid), new QuizExecutionRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidQuizId_ExpectedNotFoundException()
        {
            // Arrange
            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<QuizExecutionRequestDto>()));

            quizRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            quizExecutionService = new QuizExecutionService(null, validationProviderMock.Object, quizRepoMock.Object, 
                null, null);

            // Act
            async Task test() => await quizExecutionService.UpdateAsync(Guid.NewGuid(), new QuizExecutionRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await quizExecutionService.DeleteAsync(id);

            // Assert
            quizExecutionRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidQuizExecutionId_ExpectedException()
        {
            // Act
            async Task a() => await quizExecutionService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
