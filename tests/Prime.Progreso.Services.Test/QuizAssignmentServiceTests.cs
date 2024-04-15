using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class QuizAssignmentServiceTests
    {
        private readonly Mock<IQuizAssignmentRepository> _repositoryMock;
        private readonly Mock<IValidatorFactory> _validatorFactoryMock;
        private readonly Mock<IValidationProvider> _validationProviderMock;
        private readonly Mock<IQuizRepository> _quizRepositoryMock;
        private readonly Mock<IUserDetailsProvider> _userDetailsProviderMock;

        private IQuizAssignmentService _service;

        public QuizAssignmentServiceTests()
        {
            _repositoryMock = new();
            _validationProviderMock = new();
            _quizRepositoryMock = new();
            _userDetailsProviderMock = new();

            _repositoryMock
                .Setup(s => s.CreateAsync<QuizAssignmentRequestDto, QuizAssignmentResponseDto>(
                    It.IsAny<QuizAssignmentRequestDto>()))
                .ReturnsAsync(new QuizAssignmentResponseDto());
            _repositoryMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _repositoryMock
                .Setup(s => s.GetByIdAsync<QuizAssignmentResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizAssignmentResponseDto());
            _repositoryMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<QuizAssignmentRequestDto>()));


            _validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<QuizAssignmentRequestDto>()));

            _service = new QuizAssignmentService(_repositoryMock.Object, 
                _validationProviderMock.Object, _quizRepositoryMock.Object, _userDetailsProviderMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidQuizAssignmentId_ExpectedException()
        {
            // Act
            async Task a() => await _service.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var validQuiz = new QuizResponseDto()
            {
                Duration = 10,
                Id = id,
                Title = "Test",
                QuestionIds = new()
            };
            var quizAssignmentRequestDto = new QuizAssignmentRequestDto() 
            { 
                AssigneeId = default, 
                QuizId = validQuiz.Id,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(validQuiz.Duration),
            };

            _quizRepositoryMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(validQuiz);

            // Act
            await _service.UpdateAsync(id, quizAssignmentRequestDto);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync<QuizAssignmentRequestDto>(id, quizAssignmentRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidQuizId_ExpectedNotFoundException()
        {
            // Arrange
            // Act
            async Task test() => await _service.UpdateAsync(Guid.NewGuid(), new QuizAssignmentRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var validQuiz = new QuizResponseDto()
            {
                Duration = 10,
                Id = id,
                Title = "Test",
                QuestionIds = new()
            };
            var quizAssignmentRequestDto = new QuizAssignmentRequestDto()
            {
                AssigneeId = default,
                QuizId = validQuiz.Id,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(validQuiz.Duration),
            };

            _quizRepositoryMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(validQuiz);

            // Act
            await _service.CreateAsync(quizAssignmentRequestDto);

            // Assert
            _repositoryMock.Verify(r => r.CreateAsync<QuizAssignmentRequestDto,QuizAssignmentResponseDto>(quizAssignmentRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidQuizId_ExpectedNotFoundException()
        {
            // Arrange
            // Act
            async Task test() => await _service.CreateAsync(new QuizAssignmentRequestDto());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithIntern_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new QuizAssignmentResponseDto() { Id = id };
            string role = "ROLE_INTERN";
            int userId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            _repositoryMock
                .Setup(s => s.GetByIdAsync<QuizAssignmentResponseDto>(It.IsAny<Guid>(), userId))
                .ReturnsAsync(new QuizAssignmentResponseDto() { Id = id });

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repositoryMock.Verify(r => r.GetByIdAsync<QuizAssignmentResponseDto>(id, userId), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataLoggedWithAdminOrMentor_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new QuizAssignmentResponseDto() { Id = id };
            string role = "ROLE_ADMIN";
            int userId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            _repositoryMock
                .Setup(s => s.GetByIdAsync<QuizAssignmentResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new QuizAssignmentResponseDto() { Id = id });

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repositoryMock.Verify(r => r.GetByIdAsync<QuizAssignmentResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidQuizAssignmentId_ExpectedException()
        {
            string role = "ROLE_INTERN";
            int userId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Act
            async Task test() => await _service.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithIntern_ExpectedEmptyCollection()
        {
            string role = "ROLE_INTERN";
            int userId = 1;
            var pagingInfo = new PagingInfo();

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Arrange
            var page = new PaginatedResult<QuizAssignmentResponseDto>(new List<QuizAssignmentResponseDto>(), 0, 0, 0);

            _repositoryMock
                  .Setup(s => s.GetPageByFilterAsync(It.IsAny<PagingInfo>(), It.IsAny<int>()))
                  .ReturnsAsync(page);

            // Act
            var quizAssignments = await _service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(quizAssignments);
            Assert.Empty(quizAssignments.Content);
            _repositoryMock.Verify(r => r.GetPageByFilterAsync(pagingInfo, userId), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersLoggedWithAdminOrMentor_ExpectedEmptyCollection()
        {
            string role = "ROLE_ADMIN";
            int userId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            // Arrange
            var page = new PaginatedResult<QuizAssignmentResponseDto>(new List<QuizAssignmentResponseDto>(), 0, 0, 0);

            _repositoryMock
                .Setup(s => s.GetPageAsync<QuizAssignmentResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var quizAssignments = await _service.GetPageAsync(new PagingInfo());

            // Assert
            Assert.NotNull(quizAssignments);
            Assert.Empty(quizAssignments.Content);
            _repositoryMock.Verify(r => r.GetPageAsync<QuizAssignmentResponseDto>(0, 10, null), Times.Once());
        }
    }
}
