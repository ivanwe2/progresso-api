using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ILanguageService
    {
        Task<LanguageResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<LanguageResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<LanguageResponseDto, bool>> filter = null);
        Task<LanguageResponseDto> CreateAsync(LanguageRequestDto dto);
        Task UpdateAsync(Guid id, LanguageRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
