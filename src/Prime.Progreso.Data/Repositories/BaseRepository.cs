using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository
        where TEntity: BaseEntity
    {
        protected IMapper Mapper { get; }
        protected ProgresoDbContext DbContext { get; }
        protected DbSet<TEntity> Items { get; }

        public BaseRepository(ProgresoDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Items = DbContext.Set<TEntity>();
            Mapper = mapper;
        }

        public virtual async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var output = await Mapper.ProjectTo<TOutput>(Items.AsNoTracking()
                .Where(c => c.Id == id))
                .FirstOrDefaultAsync();

            if (output is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            return output;
        }

        public virtual async Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber, int pageSize, Expression<Func<TOutput, bool>> filter = null)
        {
            var query = Items.AsQueryable();

            if (filter != null)
                query = query.Where(Mapper.Map<Expression<Func<TEntity, bool>>>(filter));

            return await Mapper.ProjectTo<TOutput>(query).PaginateAsync(pageNumber, pageSize);
        }

        public virtual async Task<TOutput> CreateAsync<TInput, TOutput>(TInput dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            await Items.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return await Mapper.ProjectTo<TOutput>(Items.AsNoTracking()
                .Where(c => c.Id == entity.Id)).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync<TInput>(Guid id, TInput dto)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> HasAnyAsync(Guid id)
        {
            if(await Items.AnyAsync(x => x.Id == id)) return true;

            return false;
        }
    }
}
