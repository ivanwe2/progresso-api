using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.TestCase;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ITestCaseService
    {
        Task<TestCaseResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<TestCaseResponseDto>> GetPageAsync(TestCasesPagingInfo pagingInfo);
       Task<TestCaseResponseDto> CreateAsync(TestCaseRequestDto dto);
        Task UpdateAsync(Guid id, TestCaseRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
