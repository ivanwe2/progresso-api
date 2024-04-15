using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ICurriculumItemService
    {
        Task<CurriculumItemResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<CurriculumItemResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<CurriculumItemResponseDto, bool>> filter = null);
        Task<CurriculumItemResponseDto> CreateAsync(CurriculumItemRequestDto dto);
        Task UpdateAsync(Guid id, CurriculumItemRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
