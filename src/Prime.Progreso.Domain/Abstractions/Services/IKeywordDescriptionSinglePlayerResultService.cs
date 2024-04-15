using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordDescriptionSinglePlayerResultService
    {
        Task<KeywordDescriptionSinglePlayerResultResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<KeywordDescriptionSinglePlayerResultResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<KeywordDescriptionSinglePlayerResultResponseDto, bool>> filter = null);
        Task<KeywordDescriptionSinglePlayerResultResponseDto> CreateAsync(
            KeywordDescriptionSinglePlayerResultRequestDto dto);
        Task UpdateAsync(Guid id, KeywordDescriptionSinglePlayerResultRequestDto dto);
        Task DeleteAsync(Guid id);       
    }
}
