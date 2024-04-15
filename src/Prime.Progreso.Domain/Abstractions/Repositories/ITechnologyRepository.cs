using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Technology;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ITechnologyRepository : IBaseRepository
    {
        Task<PaginatedResult<TechnologyResponseDto>> GetPageByFilterAsync(TechnologiesPagingInfo pagingInfo);
        Task CheckForUniqueName(string name);
    }
}
