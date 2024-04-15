using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<ActivityResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<ActivityResponseDto, bool>> filter = null);
        Task<ActivityResponseDto> CreateAsync(ActivityRequestDto dto);
        Task UpdateAsync(Guid id, ActivityRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
