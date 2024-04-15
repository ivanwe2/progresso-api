using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Data.Repositories
{
    public class BpmnDiagramRepository : IBpmnDiagramRepository
    {
        protected IMapper Mapper { get; }
        protected ProgresoDbContext DbContext { get; }
        protected DbSet<BpmnDiagram> Items { get; }

        public BpmnDiagramRepository(ProgresoDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Items = DbContext.Set<BpmnDiagram>();
            Mapper = mapper;
        }

        public async Task<TOutput> CreateAsync<TOutput>(BpmnDiagramCreateRequestDto dto, string filePath)
        {
            var entity = Mapper.Map<BpmnDiagram>(dto);

            entity.FilePath = filePath;

            Items.Add(entity);
            await DbContext.SaveChangesAsync();

            return Mapper.Map<TOutput>(entity);
        }

        public async Task<string> DeleteAsync(Guid id)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.FileId == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(BpmnDiagram).Name} was not found!");
            }

            Items.Remove(entity);
            await DbContext.SaveChangesAsync();

            return entity.FilePath;
        }

        public async Task<BpmnDiagramGetFileResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await Items.FirstOrDefaultAsync(x => x.FileId == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(BpmnDiagram).Name} was not found!");
            }

            return Mapper.Map<BpmnDiagramGetFileResponseDto>(entity);
        }

        public async Task UpdateAsync(Guid id, BpmnDiagramUpdateRequestDto dto)
        {
            var entity = await Items.FirstOrDefaultAsync(x => x.FileId == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(BpmnDiagram).Name} was not found!");
            }

            entity.AuthorId = dto.AuthorId;
            entity.UploadDate = DateTime.Now;

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber, int pageSize, Expression<Func<TOutput, bool>> filter = null)
        { 
            var query = Items.AsQueryable();

            if (filter != null)
                query = query.Where(Mapper.Map<Expression<Func<BpmnDiagram, bool>>>(filter));

            return await Mapper.ProjectTo<TOutput>(query).PaginateAsync(pageNumber, pageSize);
        }

        public async Task TryValidateNewFileName(string newFileName,string fileType)
        {
            if (newFileName == null || newFileName == string.Empty) 
                throw new ArgumentNullException("File name was null!");
            
            if(await Items.AnyAsync(i => i.FileName == newFileName + fileType))
                throw new InvalidFileNameException();
        }

        public async Task<bool> IsAccessAllowedAsync(Guid id, int userId)
        {
            return await Items.AnyAsync(x => x.FileId == id && x.AuthorId == userId);
        }

        public async Task<PaginatedResult<BpmnDiagramGetMetadataResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var query = Items.AsQueryable();

            query = query.Where(m => m.AuthorId == userId);

            return await Mapper.ProjectTo<BpmnDiagramGetMetadataResponseDto>(query).PaginateAsync(pageNumber, pageSize);
        }
    }
}
