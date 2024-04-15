using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Curriculum;
using System.Linq.Expressions;

namespace Prime.Progreso.Data.Repositories
{
    public class CurriculumRepository : BaseRepository<Curriculum>, ICurriculumRepository
    {
        public CurriculumRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public override async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var entity = await Items
                                .AsNoTracking()
                                .Include(c=>c.CurriculumItems)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                throw new NotFoundException($"{typeof(Curriculum).Name} was not found!");
            }
            return Mapper.Map<TOutput>(entity);
        }

        public async Task<PaginatedResult<TOutput>> GetPageByFilterAsync<TOutput>(CurriculumsPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.technologyIds.Any())
            {
                query = FilterByTechnology(query, pagingInfo.technologyIds);
            }

            return await Mapper.ProjectTo<TOutput>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        private IQueryable<Curriculum> FilterByTechnology(IQueryable<Curriculum> query, List<Guid> technologyIdsFilter)
        {
            return query.Where(c => technologyIdsFilter.Contains(c.TechnologyId));
        }
    }
}
