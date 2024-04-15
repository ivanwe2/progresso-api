using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Technology;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class TechnologyServiceTests
    {
        public ITechnologyService technologyService;
        public Mock<ITechnologyRepository> technologyRepoMock;
        public Mock<IValidationProvider> validationProviderMock;
        public Mock<ICodingChallengeRepository> challengeRepoMock;

        public TechnologyServiceTests()
        {
            technologyRepoMock = new Mock<ITechnologyRepository>();
            validationProviderMock = new Mock<IValidationProvider>();
            challengeRepoMock = new Mock<ICodingChallengeRepository>();

            technologyRepoMock
                .Setup(s => s.CreateAsync<TechnologyRequestDto, TechnologyResponseDto>(It.IsAny<TechnologyRequestDto>()))
                .ReturnsAsync(new TechnologyResponseDto());
            technologyRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            technologyRepoMock
                .Setup(s => s.GetByIdAsync<TechnologyResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new TechnologyResponseDto());
            technologyRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TechnologyRequestDto>()));

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<TechnologyRequestDto>()));

            challengeRepoMock
                .Setup(s=> s.HasAnyRelatedToTechnologyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            technologyService = new TechnologyService(technologyRepoMock.Object,
                validationProviderMock.Object, challengeRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var technologyRequestDto = new TechnologyRequestDto();

            var expected = new TechnologyResponseDto();

            // Act
            var result = await technologyService.CreateAsync(technologyRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            technologyRepoMock.Verify(r => 
                r.CreateAsync<TechnologyRequestDto, TechnologyResponseDto>(technologyRequestDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var technologyRequestDto = new TechnologyRequestDto()
            {
                Name = "Test"
            };

            validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<TechnologyRequestDto>()))
                .Throws(new ValidationException("sth"));

            technologyService = new TechnologyService(technologyRepoMock.Object,
                validationProviderMock.Object, challengeRepoMock.Object);

            // Act
            async Task a() => await technologyService.CreateAsync(technologyRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new TechnologyResponseDto() { Id = id };

            technologyRepoMock
                .Setup(s => s.GetByIdAsync<TechnologyResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new TechnologyResponseDto() { Id = id });

            technologyService = new TechnologyService(technologyRepoMock.Object,
                validationProviderMock.Object, challengeRepoMock.Object);

            // Act
            var result = await technologyService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            technologyRepoMock.Verify(r => r.GetByIdAsync<TechnologyResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidTechnologyId_ExpectedException()
        {
            // Act
            async Task a() => await technologyService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<TechnologyResponseDto>(new List<TechnologyResponseDto>(), 0, 0, 0);

            technologyRepoMock
                .Setup(s => s.GetPageByFilterAsync(It.IsAny<TechnologiesPagingInfo>()))
                .ReturnsAsync(page);

            var pagingInfo = new TechnologiesPagingInfo();

            // Act
            var technologies = await technologyService.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(technologies);
            Assert.Empty(technologies.Content);
            technologyRepoMock.Verify(r => r.GetPageByFilterAsync(pagingInfo), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepsoitoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var technologyRequestDto = new TechnologyRequestDto();

            // Act
            await technologyService.UpdateAsync(id, technologyRequestDto);

            // Assert
            technologyRepoMock.Verify(r => r.UpdateAsync<TechnologyRequestDto>(id, technologyRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidTechnologyId_ExpectedException()
        {
            // Act
            async Task a() => await technologyService.UpdateAsync(default(Guid), new TechnologyRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            challengeRepoMock
                .Setup(s => s.HasAnyRelatedToTechnologyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            await technologyService.DeleteAsync(id);

            // Assert
            technologyRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidTechnologyId_ExpectedException()
        {
            // Act
            async Task a() => await technologyService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_TechnologyIsRelatedToCodingChallenges_ExpectedException()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            async Task a() => await technologyService.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(a);
        }
    }
}
