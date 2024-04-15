using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Technology;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ITechnologyService
    {
        Task<TechnologyResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<TechnologyResponseDto>> GetPageAsync(TechnologiesPagingInfo pagingInfo);
        Task<TechnologyResponseDto> CreateAsync(TechnologyRequestDto dto);
        Task UpdateAsync(Guid id, TechnologyRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
