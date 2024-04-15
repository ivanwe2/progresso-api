using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Curriculum;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ICurriculumRepository : IBaseRepository
    {
        Task<PaginatedResult<TOutput>> GetPageByFilterAsync<TOutput>(CurriculumsPagingInfo pagingInfo);
    }
}
