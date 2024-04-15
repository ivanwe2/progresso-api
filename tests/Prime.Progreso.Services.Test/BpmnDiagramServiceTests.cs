using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestModels.BpmnDiagram;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class BpmnDiagramServiceTests
    {
        private readonly Mock<IBpmnDiagramRepository> _repositoryMock;
        private readonly Mock<IBpmnDiagramFileRepository> _fileHandlerMock;
        private readonly Mock<IUserDetailsProvider> _userDetailsProviderMock;
        private IBpmnDiagramService _service;
        


        private BpmnDiagramCreateRequestForm validCreateForm;
        private BpmnDiagramCreateRequestForm invalidCreateForm;
        private BpmnDiagramUpdateRequestForm validUpdateForm;
        private BpmnDiagramUpdateRequestForm invalidUpdateForm;
        private string role;
        private int authorId;

        private FileStreamResult expectedFileStreamResult;

        public BpmnDiagramServiceTests()
        {
            _repositoryMock = new Mock<IBpmnDiagramRepository>();
            _fileHandlerMock = new Mock<IBpmnDiagramFileRepository>();
            _userDetailsProviderMock = new();

            _repositoryMock
                .Setup(s => s.CreateAsync<BpmnDiagramGetMetadataResponseDto>(It.IsAny<BpmnDiagramCreateRequestDto>(),It.IsAny<string>()))
                .ReturnsAsync(new BpmnDiagramGetMetadataResponseDto());
            _repositoryMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _repositoryMock
                .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new BpmnDiagramGetFileResponseDto());
            _repositoryMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BpmnDiagramUpdateRequestDto>()));
            _repositoryMock
                .Setup(s => s.TryValidateNewFileName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _repositoryMock
                .Setup(s => s.IsAccessAllowedAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);
                
            _service = new BpmnDiagramService(_repositoryMock.Object, _fileHandlerMock.Object, _userDetailsProviderMock.Object);

            var fileName = "test.pdf";
            var stream = new MemoryStream();

            expectedFileStreamResult = new FileStreamResult(stream, "application/xml");

            var file = new FormFile(stream, 0, stream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            validCreateForm = new BpmnDiagramCreateRequestForm()
            {
                FileName = fileName,
                BpmnDiagramFile = file
            };
            invalidCreateForm = new BpmnDiagramCreateRequestForm()
            {
                FileName = fileName,
                BpmnDiagramFile = null
            };
            validUpdateForm = new BpmnDiagramUpdateRequestForm()
            {
                BpmnDiagramFile = file
            };
            invalidUpdateForm = new BpmnDiagramUpdateRequestForm()
            {
                BpmnDiagramFile = null
            };
            role = "ROLE_ADMIN";
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNewResponseDto()
        {
            // Arrange
            var expected = new BpmnDiagramGetMetadataResponseDto();

            // Act
            var result = await _service.CreateAsync(validCreateForm);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repositoryMock.Verify(r => r.CreateAsync<BpmnDiagramGetMetadataResponseDto>(It.IsAny<BpmnDiagramCreateRequestDto>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidFile_ExpectedException()
        {
            // Arrange
            // Act
            async Task test() => await _service.CreateAsync(invalidCreateForm);

            // Assert
            await Assert.ThrowsAsync<FileIsNullException>(test);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ExpectedNoErrors()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ValidIdWithMentorRole_ExpectedNoErrors()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            role = "ROLE_MENTOR";
            authorId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(authorId);

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ValidIdWithMentorRoleForbiddenAccess_ExpectedUnauthorizedAccessException()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            role = "ROLE_MENTOR";

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);

            _repositoryMock
                .Setup(s => s.IsAccessAllowedAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            async Task test() => await _service.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(test);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ExpectedArgumentNullException()
        {
            // Arrange
            Guid id = Guid.Empty;

            // Act
            async Task test() => await _service.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetFileByIdAsync_ValidId_ExpectedNoErrors()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var streamReader = new StreamReader(expectedFileStreamResult.FileStream);

            _fileHandlerMock
               .Setup(s => s.ReturnFileAsStream(It.IsAny<string>()))
               .Returns(streamReader);

            // Act
            var result = await _service.GetFileByIdAsync(id);

            // Assert
            Assert.Equivalent(streamReader, result);
            _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public async Task GetFileByIdAsync_ValidIdWithMentorRoleForbiddenAccess_ExpectedUnauthorizedAccessException()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _repositoryMock
               .Setup(s => s.IsAccessAllowedAsync(It.IsAny<Guid>(), It.IsAny<int>()))
               .ReturnsAsync(false);

            role = "ROLE_MENTOR";

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);

            // Act
            async Task test() => await _service.GetFileByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(test);
        }

        [Fact]
        public async Task GetFileByIdAsync_ValidIdWithMentorRole_ExpectedNoErrors()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var streamReader = new StreamReader(expectedFileStreamResult.FileStream);

            _fileHandlerMock
               .Setup(s => s.ReturnFileAsStream(It.IsAny<string>()))
               .Returns(streamReader);

            role = "ROLE_MENTOR";
            authorId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(authorId);

            // Act
            var result = await _service.GetFileByIdAsync(id);

            // Assert
            Assert.Equivalent(streamReader, result);
            _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ExpectedArgumentNullException()
        {
            // Arrange
            Guid id = Guid.Empty;

            // Act
            async Task test() => await _service.GetFileByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<BpmnDiagramGetMetadataResponseDto>(new List<BpmnDiagramGetMetadataResponseDto>(), 0, 0, 0);

            _repositoryMock
                .Setup(s => s.GetPageAsync<BpmnDiagramGetMetadataResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var activities = await _service.GetMetadataPageAsync(1, 10);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            _repositoryMock.Verify(r => r.GetPageAsync<BpmnDiagramGetMetadataResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersWithMentorRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<BpmnDiagramGetMetadataResponseDto>(new List<BpmnDiagramGetMetadataResponseDto>(), 0, 0, 0);

            _repositoryMock
                .Setup(s => s.GetPageAndFilterByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(page);

            role = "ROLE_MENTOR";
            authorId = 10;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(authorId);

            // Act
            var activities = await _service.GetMetadataPageAsync(1, 10);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            _repositoryMock.Verify(r => r.GetPageAndFilterByUserIdAsync(1, 10, authorId), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedNewResponseDto()
        {
            // Arrange
            _fileHandlerMock
                .Setup(s => s.UpdateExistingFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("test");

            // Act
            await _service.UpdateAsync(Guid.NewGuid(), validUpdateForm);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Guid>(),It.IsAny<BpmnDiagramUpdateRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidDataWithMentorRoleForbiddenAccess_ExpectedUnauthorizedAccessException()
        {
            // Arrange
            _repositoryMock
              .Setup(s => s.IsAccessAllowedAsync(It.IsAny<Guid>(), It.IsAny<int>()))
              .ReturnsAsync(false);

            role = "ROLE_MENTOR";
            authorId = 1;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(authorId);

            // Act
            async Task test() => await _service.UpdateAsync(Guid.NewGuid(), validUpdateForm);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(test);
        }

        [Fact]
        public async Task UpdateAsync_ValidDataWithMentorRole_ExpectedNewResponseDto()
        {
            // Arrange
            _fileHandlerMock
                .Setup(s => s.UpdateExistingFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("test");

            role = "ROLE_MENTOR";
            var id = Guid.NewGuid();
            authorId = 10;

            _userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            _userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(authorId);

            // Act
            await _service.UpdateAsync(id, validUpdateForm);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(id, It.IsAny<BpmnDiagramUpdateRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidFile_ExpectedException()
        {
            // Act
            async Task test() => await _service.UpdateAsync(Guid.NewGuid(), invalidUpdateForm);

            // Assert
            await Assert.ThrowsAsync<FileIsNullException>(test);
        }
        [Fact]
        public async Task UpdateAsync_InvalidId_ExpectedArgumentNullException()
        {
            // Arrange
            Guid id = Guid.Empty;

            // Act
            async Task test() => await _service.UpdateAsync(id, null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }
    }
}
