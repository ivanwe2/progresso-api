using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordService
    {
        Task<KeywordResponseDto> CreateAsync(KeywordRequestDto dto);
        Task UpdateAsync(Guid id, KeywordRequestDto dto);
        Task DeleteAsync(Guid id);
        Task<KeywordResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<KeywordResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<RandomKeywordResponseDto> GetRandomKeywordAsync(RandomKeywordRequestDto dto);
    }
}
