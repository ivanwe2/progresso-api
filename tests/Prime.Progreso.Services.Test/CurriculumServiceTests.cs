using Moq;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Curriculum;
using Prime.Progreso.Domain.Pagination.Technology;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.Curriculum;
using Xunit;
using IValidatorFactory = Prime.Progreso.Domain.Abstractions.Factories.IValidatorFactory;
using ValidationException = Prime.Progreso.Domain.Exceptions.ValidationException;

namespace Prime.Progreso.Services.Test
{
    public class CurriculumServiceTests
    {
        public Mock<ICurriculumRepository> _repositoryMock;
        public Mock<IValidatorFactory> _validatorFactoryMock;
        public ICurriculumService _curriculumService;
        public IValidationProvider _validationProvider; 
        public Mock<ITechnologyRepository> _technologyRepositoryMock;

        public CurriculumRequestDto validCurriculumRequestDto;

        public CurriculumServiceTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();
            _technologyRepositoryMock = new Mock<ITechnologyRepository>();

            _repositoryMock
                .Setup(s => s.CreateAsync<CurriculumRequestDto, CurriculumResponseDto>(It.IsAny<CurriculumRequestDto>()))
                .ReturnsAsync(new CurriculumResponseDto());
            _repositoryMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _repositoryMock
                .Setup(s => s.GetByIdAsync<CurriculumResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new CurriculumResponseDto());
            _repositoryMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CurriculumRequestDto>()));

            _validatorFactoryMock
                .Setup(s => s.GetValidator<CurriculumRequestDto>())
                .Returns(new CurriculumRequestDtoValidator());

            _validationProvider = new ValidationProvider(_validatorFactoryMock.Object);

            _curriculumService = new CurriculumService(_repositoryMock.Object,_validationProvider,_technologyRepositoryMock.Object);

            validCurriculumRequestDto = new CurriculumRequestDto()
            {
                TechnologyId = Guid.NewGuid(),
                Description = "description",
                Duration = 1
            };
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var expected = new CurriculumResponseDto();

            _technologyRepositoryMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            // Act
            var result = await _curriculumService.CreateAsync(validCurriculumRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var InvalidCurriculumRequestDto = new CurriculumRequestDto();

            // Act
            async Task test() => await _curriculumService.CreateAsync(InvalidCurriculumRequestDto);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task CreateAsync_InvalidTechnologyId_ExpectedException()
        {
            // Arrange
            var InvalidCurriculumRequestDto = new CurriculumRequestDto();

            _technologyRepositoryMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            // Act
            async Task test() => await _curriculumService.CreateAsync(validCurriculumRequestDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _curriculumService.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ExpectedException()
        {
            // Act
            async Task test() => await _curriculumService.DeleteAsync(default);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidIdGiven_ExpectedException()
        {
            // Act
            async Task test() => await _curriculumService.GetByIdAsync(default);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task GetByIdAsync_ValidIdGiven_ExpectedNormalBehaviour()
        {
            // Arrange
            var curriculum = new CurriculumResponseDto();
            _repositoryMock
                .Setup(s => s.GetByIdAsync<CurriculumResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(curriculum);

            Guid id = Guid.NewGuid();

            // Act
            var result = await _curriculumService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(curriculum, result);
        }

        [Fact]
        public async Task GetAllAsync_NoTechnologyFilter_ExpectedEmptyCollection()
        {
            // Arrange
            var pagedResultCurriculums = new PaginatedResult<CurriculumResponseDto>(new List<CurriculumResponseDto>(), 0, 0, 0);
            var pagedResultTechnologies = new PaginatedResult<TechnologyResponseDto>(new List<TechnologyResponseDto>(), 0, 0, 0);

            _technologyRepositoryMock
                .Setup(s => s.GetPageByFilterAsync(It.IsAny<TechnologiesPagingInfo>()))
                .ReturnsAsync(pagedResultTechnologies);

            _repositoryMock
                .Setup(s => s.GetPageByFilterAsync<CurriculumResponseDto>(It.IsAny<CurriculumsPagingInfo>()))
                .ReturnsAsync(pagedResultCurriculums);


            // Act
            var result = await _curriculumService.GetPageAsync(new CurriculumsPagingInfo());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResultCurriculums,result);

        }

        [Fact]
        public async Task GetAllAsync_TechnologyFilter_ExpectedEmptyCollection()
        {
            // Arrange
            var pagedResultCurriculums = PaginatedResult<CurriculumResponseDto>.EmptyResult(0, 0);

            var testTechnology = new TechnologyResponseDto() { Id = Guid.NewGuid(), Name = "test" };
            var pagedResultTechnologies = new PaginatedResult<TechnologyResponseDto>(new List<TechnologyResponseDto>() { testTechnology }, 0, 0, 0);

            _technologyRepositoryMock
                .Setup(s => s.GetPageByFilterAsync(It.IsAny<TechnologiesPagingInfo>()))
                .ReturnsAsync(pagedResultTechnologies);

            _repositoryMock
                .Setup(s => s.GetPageByFilterAsync<CurriculumResponseDto>(It.IsAny<CurriculumsPagingInfo>()))
                .ReturnsAsync(pagedResultCurriculums);

            // Act
            var result = await _curriculumService.GetPageAsync(new CurriculumsPagingInfo() { TechnologyFilter = "test"});

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResultCurriculums, result);
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ExpectedException()
        {
            // Arrange
            // Act
            async Task test() => await _curriculumService.UpdateAsync(default, new CurriculumRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            // Act
            async Task test() => await _curriculumService.UpdateAsync(Guid.NewGuid(), new CurriculumRequestDto());

            // Assert
            await Assert.ThrowsAsync<ValidationException>(test);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _technologyRepositoryMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            await _curriculumService.UpdateAsync(id, validCurriculumRequestDto);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(id, validCurriculumRequestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidTechnologyId_ExpectedException()
        {
            // Arrange
            _technologyRepositoryMock.Setup(r => r.HasAnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            async Task test() => await _curriculumService.CreateAsync(validCurriculumRequestDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(test);
        }
    }
}
