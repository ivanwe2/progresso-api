using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IKeywordSinglePlayerResultRepository : IBaseRepository
    {
        Task<PaginatedResult<KeywordSinglePlayerResultResponseDto>> GetPageAsync(
            KeywordSinglePlayerResultPagingInfo pagingInfo);
    }
}
