using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordSinglePlayerResultService
    {
        Task<KeywordSinglePlayerResultResponseDto> CreateAsync(KeywordSinglePlayerResultRequestDto dto);
        Task UpdateIsCorrectAsync(Guid id, KeywordSinglePlayerResultIsCorrectUpdateRequestDto dto);
        Task<PaginatedResult<KeywordSinglePlayerResultResponseDto>> GetPageAsync(
            KeywordSinglePlayerResultPagingInfo pagingInfo);
    }
}
