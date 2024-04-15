using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.TestCase;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ITestCaseRepository : IBaseRepository
    {
        Task<PaginatedResult<TestCaseResponseDto>> GetPageAsync(TestCasesPagingInfo pagingInfo);
    }
}
