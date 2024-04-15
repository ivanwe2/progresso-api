using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Technology;
using System.Linq.Expressions;

namespace Prime.Progreso.Data.Repositories
{
    public class TechnologyRepository : BaseRepository<Technology>, ITechnologyRepository
    {
        public TechnologyRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<PaginatedResult<TechnologyResponseDto>> GetPageByFilterAsync(TechnologiesPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.PartOfNameFilter != string.Empty)
            {
                query = query.Where(t => EF.Functions.Like(t.Name,$"%{pagingInfo.PartOfNameFilter}%"));
            }

            return await Mapper.ProjectTo<TechnologyResponseDto>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task CheckForUniqueName(string name)
        {
            if (await Items.AnyAsync(t => t.Name == name))
            {
                throw new EntryAlreadyExistsException($"A Technology with the name: {name}, already exists!");
            }
        }
    }
}
