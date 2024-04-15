using Moq;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Enums;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.CodingChallenge;
using Prime.Progreso.Domain.Providers;
using Prime.Progreso.Domain.Validators.CodingChallenge;
using System.Linq.Expressions;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class CodingChallengeServiceTests
    {
        private readonly Mock<ICodingChallengeRepository> repositoryMock;
        private readonly Mock<IValidatorFactory> validatorFactory;
        private readonly Mock<IAssignmentToChallengeRepository> assignmentRepoMock;
        private readonly Mock<ITechnologyRepository> technologyRepoMock;
        private readonly Mock<IUserDetailsProvider> userDetailsProviderMock;
        private IValidationProvider validationProvider;

        public CodingChallengeServiceTests()
        {
            repositoryMock = new Mock<ICodingChallengeRepository>();
            assignmentRepoMock = new Mock<IAssignmentToChallengeRepository>();
            validatorFactory = new Mock<IValidatorFactory>();
            userDetailsProviderMock = new();
            validationProvider = new ValidationProvider(validatorFactory.Object);
            technologyRepoMock = new();
        }

        private void CompareProperties(CodingChallengeResponseDto responseDto, CodingChallengeResponseDto actual)
        {
            Assert.Equal(responseDto.Id, actual.Id);
            Assert.Equal(responseDto.Title, actual.Title);
            Assert.Equal(responseDto.Description, actual.Description);
            Assert.Equal(responseDto.Codebase, actual.Codebase);
            Assert.Equal(responseDto.Type, actual.Type);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreateCodingChallenge_ReturnsTheNewCodingChallenge()
        {
            CodingChallengeRequestDto requestDto = new CodingChallengeRequestDto()
            {
                Title = "Fast coding",
                Description = "description",
                Type = CodingChallengeType.PairProgramming,
                Codebase = "https://www.regex.com",
                TechnologyId = Guid.NewGuid()
            };

            CodingChallengeResponseDto responseDto = new CodingChallengeResponseDto()
            {
                Id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14"),
                Title = "Fast coding",
                Description = "description",
                Type = CodingChallengeType.PairProgramming,
                Codebase = "https://www.regex.com",
                Technology = new TechnologyResponseDto()
                
            };

            CodingChallengeRequestDtoValidator validator = new CodingChallengeRequestDtoValidator();


            validatorFactory.Setup(s => s.GetValidator<CodingChallengeRequestDto>())
                .Returns(validator);

            repositoryMock.Setup(s => s.CreateAsync<CodingChallengeRequestDto, CodingChallengeResponseDto>(requestDto))
                .ReturnsAsync(responseDto);

            technologyRepoMock.Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object,
                technologyRepoMock.Object, null);

            var actual = await service.CreateAsync(requestDto);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.CreateAsync<CodingChallengeRequestDto, CodingChallengeResponseDto>(requestDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_SuccessfullyUpdatedCodingChallenge_ReturnsTheUpdatedCodingChallenge()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            CodingChallengeRequestDto requestDto = new CodingChallengeRequestDto()
            {
                Title = "Fast coding",
                Description = "description",
                Type = CodingChallengeType.PairProgramming,
                Codebase = "https://www.regex.com",
                TechnologyId = Guid.NewGuid()
            };

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object,
                technologyRepoMock.Object, null);

            CodingChallengeRequestDtoValidator validator = new CodingChallengeRequestDtoValidator();

            validatorFactory.Setup(s => s.GetValidator<CodingChallengeRequestDto>())
             .Returns(validator);

            repositoryMock.Setup(s => s.UpdateAsync(id, requestDto))
                .Returns(Task.CompletedTask);

            technologyRepoMock.Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            await service.UpdateAsync(id, requestDto);

            repositoryMock.Verify(x => x.UpdateAsync(id, requestDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfullyDeleteCodingChallenge_VerifyThatDeleteWasInvockedOnce()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object, 
                null, null);

            repositoryMock.Setup(s => s.DeleteAsync(id))
             .Returns(Task.CompletedTask);

            await service.DeleteAsync(id);

            repositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_SuccessfullyGetCodingChallengeById_ReturnASpecificCodingChallengeById()
        {
            Guid id = Guid.Parse("5F296D4C-FB60-4510-B523-F6151E7DED14");
            var userId = 1;
            var role = "ROLE_ADMIN";

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var responseDto = new CodingChallengeResponseDto();

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object, null, 
                userDetailsProviderMock.Object);

            repositoryMock.Setup(s => s.GetByIdAsync<CodingChallengeResponseDto>(id))
              .ReturnsAsync(responseDto);

            var actual = await service.GetByIdAsync(id);

            CompareProperties(responseDto, actual);

            repositoryMock.Verify(r => r.GetByIdAsync<CodingChallengeResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidCodingChallengeId_ExpectedException()
        {
            //Arrange
            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object,
                null,null);

            // Act
            async Task a() => await service.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidDataWithInternRole_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new CodingChallengeResponseDto() { Id = id };
            var role = "ROLE_INTERN";
            var userId = 10;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = null
            };

            repositoryMock
                .Setup(s => s.GetByIdAsync<CodingChallengeResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new CodingChallengeResponseDto() { Id = id });

            assignmentRepoMock
                .Setup(r => r.HasAnyAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            assignmentRepoMock
                .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(assignment);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, assignmentRepoMock.Object, null,
                userDetailsProviderMock.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            repositoryMock.Verify(r => r.GetByIdAsync<CodingChallengeResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_AssignmentStartTimeAfterCurrentTimeWithInternRole_ExpectedException()
        {
            // Arrange
            var role = "ROLE_INTERN";
            var userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var id = Guid.NewGuid();
            var codingChallengeResponseDto = new CodingChallengeResponseDto
            {
                Id = id
            };

            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(+5),
                EndTime = null
            };

            repositoryMock
              .Setup(r => r.GetByIdAsync<CodingChallengeResponseDto>(It.IsAny<Guid>()))
              .ReturnsAsync(codingChallengeResponseDto);

            assignmentRepoMock
               .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
               .ReturnsAsync(assignment);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, 
                assignmentRepoMock.Object, null,userDetailsProviderMock.Object);

            // Act
            async Task a() => await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(a);
        }


        [Fact]
        public async Task GetByIdAsync_AssignmentEndTimeBeforeCurrentTimeWithInternRole_ExpectedException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var codingChallengeResponseDto = new CodingChallengeResponseDto
            {
                Id = id
            };

            var role = "ROLE_INTERN";
            var userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var assignment = new AssignmentResponseDto
            {
                StartTime = DateTime.Now.AddDays(-5),
                EndTime = DateTime.Now.AddDays(-4)
            };

            repositoryMock
                .Setup(r => r.GetByIdAsync<CodingChallengeResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(codingChallengeResponseDto);

            assignmentRepoMock
               .Setup(s => s.GetByInternAndChallengeIdsAsync(It.IsAny<int>(), It.IsAny<Guid>()))
               .ReturnsAsync(assignment);
            assignmentRepoMock
               .Setup(r => r.HasAnyAsync(It.IsAny<Guid>(), It.IsAny<int>()))
               .ReturnsAsync(true);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, 
                assignmentRepoMock.Object, null,userDetailsProviderMock.Object);

            // Act
            async Task a() => await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(a);
        }


        [Fact]
        public async Task GetAllAsync_SuccessfullyGetAllCodingChallenges_ReturnNumberOfCodingChallengesFromChoosedPage()
        {
            int pageNumber = 1;
            int pageSize = 10;
            int count = 10;
            var role = "ROLE_ADMIN";
            var userId = 1;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var pagingInfo = new CodingChallengesPagingInfo
            {
                Page = 1,
                Size = 10
            };

            List<CodingChallengeResponseDto> responseDtos = new List<CodingChallengeResponseDto>();

            Expression<Func<CodingChallengeResponseDto, bool>> filter = null;

            PaginatedResult<CodingChallengeResponseDto> paginatedResult = new PaginatedResult<CodingChallengeResponseDto>(responseDtos, count, pageNumber, pageSize);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, 
                assignmentRepoMock.Object, null, userDetailsProviderMock.Object);

            repositoryMock.Setup(s => s.GetPageAsync(pageNumber, pageSize, filter))
                .ReturnsAsync(paginatedResult);

            var actual = await service.GetPageAsync(pagingInfo);

            for (int i = 0; i < responseDtos.Count; i++)
            {
                Assert.Equal(responseDtos[i].Title, actual.Content[i].Title);
                Assert.Equal(responseDtos[i].Description, actual.Content[i].Description);
                Assert.Equal(responseDtos[i].Codebase, actual.Content[i].Codebase);
                Assert.Equal(responseDtos[i].Type, actual.Content[i].Type);
            }

            repositoryMock.Verify(r => r.GetPageAsync<CodingChallengeResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersWithInternRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<CodingChallengeResponseDto>(new List<CodingChallengeResponseDto>(), 0, 0, 0);

            var codingChallengeIds = new List<Guid> { Guid.NewGuid() };

            var role = "ROLE_INTERN";
            var userId = 10;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var pagingInfo = new CodingChallengesPagingInfo
            {
                Page = 1,
                Size = 10,
                codingChallengeIds = new List<Guid>()
            };

            assignmentRepoMock
                .Setup(s => s.GetAssignedCodingChallengeIdsAsync(It.IsAny<int>()))
                .ReturnsAsync(codingChallengeIds);

            repositoryMock
                .Setup(s => s.GetPageAsync(It.IsAny<CodingChallengesPagingInfo>()))
                .ReturnsAsync(page);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, 
                assignmentRepoMock.Object, null, userDetailsProviderMock.Object);

            // Act
            var testCases = await service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(testCases);
            Assert.Empty(testCases.Content);
            repositoryMock.Verify(r => r.GetPageAsync(pagingInfo), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersThereAreNoAssignedCodingChallengesWithInternRole_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<CodingChallengeResponseDto>(new List<CodingChallengeResponseDto>(), 0, 0, 0);
            var emptyCodingChallengeIds = new List<Guid>();

            var role = "ROLE_INTERN";
            var userId = 10;

            userDetailsProviderMock.Setup(s => s.GetUserRole()).Returns(role);
            userDetailsProviderMock.Setup(s => s.GetUserId()).Returns(userId);

            var pagingInfo = new CodingChallengesPagingInfo
            {
                Page = 1,
                Size = 10,
                codingChallengeIds = new List<Guid>()
            };

            assignmentRepoMock
                .Setup(s => s.GetAssignedCodingChallengeIdsAsync(It.IsAny<int>()))
                .ReturnsAsync(emptyCodingChallengeIds);

            repositoryMock
                .Setup(s => s.GetPageAsync(It.IsAny<CodingChallengesPagingInfo>()))
                .ReturnsAsync(page);

            var service = new CodingChallengeService(repositoryMock.Object, validationProvider, 
                assignmentRepoMock.Object, null, userDetailsProviderMock.Object);


            // Act
            var testCases = await service.GetPageAsync(pagingInfo);

            // Assert
            Assert.NotNull(testCases);
            Assert.Empty(testCases.Content);
        }
    }
}
