using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IBaseRepository
    {
        Task<TOutput> GetByIdAsync<TOutput>(Guid id);
        Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(
           int pageNumber, int pageSize, Expression<Func<TOutput, bool>> filter = null);
        Task<TOutput> CreateAsync<TInput, TOutput>(TInput dto);
        Task UpdateAsync<TInput>(Guid id, TInput dto);
        Task DeleteAsync(Guid id);
        Task<bool> HasAnyAsync(Guid id);
    }
}
