using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.SolutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class SolutionServiceTests
    {
        private ISolutionService service;
        private readonly Mock<ISolutionFileRepository> fileRepositoryMock;
        private readonly Mock<IAssignmentToChallengeRepository> assignmentRepositoryMock;
        private readonly Mock<ICodingChallengeRepository> codingChallengeRepositoryMock;
        private readonly Mock<IValidationProvider> validationProviderMock;
        private readonly Mock<IUserDetailsProvider> userDetailsMock;

        public SolutionServiceTests()
        {
            fileRepositoryMock = new Mock<ISolutionFileRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            codingChallengeRepositoryMock = new Mock<ICodingChallengeRepository>();
            assignmentRepositoryMock = new Mock<IAssignmentToChallengeRepository>();
            userDetailsMock = new Mock<IUserDetailsProvider>();

            fileRepositoryMock
                .Setup(s => s.CreateOrUpdateSolutionFile(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()));

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<SolutionRequestDto>()));

            codingChallengeRepositoryMock
            .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            assignmentRepositoryMock
                .Setup(s => s.GetSolutionByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(new SolutionResponseDto());
            assignmentRepositoryMock
                .Setup(s => s.UpdateSolutionPathAsync(It.IsAny<int>(), It.IsAny<SolutionRequestDto>()))
                .ReturnsAsync(new SolutionResponseDto());

            service = new SolutionService(assignmentRepositoryMock.Object, codingChallengeRepositoryMock.Object,
                validationProviderMock.Object, fileRepositoryMock.Object, userDetailsMock.Object);
        }

        [Fact]
        public async Task SubmitCodeAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var solutionRequestDto = new SolutionRequestDto();

            var expected = new SolutionResponseDto();

            // Act
            var result = await service.SubmitCodeAsync(solutionRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            assignmentRepositoryMock.Verify(r => r
                .UpdateSolutionPathAsync(It.IsAny<int>(), It.IsAny<SolutionRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task SubmitCodeAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var solutionRequestDto = new SolutionRequestDto();

            validationProviderMock
                .Setup(s => s.TryValidateAsync(It.IsAny<SolutionRequestDto>()))
                .ThrowsAsync(new ValidationException("sth"));

            service = new SolutionService(assignmentRepositoryMock.Object, codingChallengeRepositoryMock.Object,
                validationProviderMock.Object, fileRepositoryMock.Object, userDetailsMock.Object);

            // Act
            async Task a() => await service.SubmitCodeAsync(solutionRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task GetCodeByCodingChallengeIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var expected = new SolutionResponseDto();

            // Act
            var result = await service.GetCodeByCodingChallengeIdAsync(default(int), id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            assignmentRepositoryMock.Verify(r => r
                .GetSolutionByInternAndChallengeIdsAsync(default(int), id), Times.Once());
        }

        [Fact]
        public async Task GetCodeByCodingChallengeIdAsync_InvalidCodingChallengeId_ExpectedException()
        {
            // Act
            async Task a() => await service.GetCodeByCodingChallengeIdAsync(default(int), default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }
    }
}
