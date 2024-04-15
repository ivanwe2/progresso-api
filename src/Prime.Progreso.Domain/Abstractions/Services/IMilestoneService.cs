using Prime.Progreso.Domain.Dtos.Milestones;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IMilestoneService
    {
        Task<MilestoneResponseDto> CreateAsync(MilestoneRequestDto dto);
        Task UpdateAsync(Guid id, MilestoneRequestDto dto);
        Task DeleteAsync(Guid id);
        Task<MilestoneResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<MilestoneResponseDto>> GetPageAsync(int pageNumber, int pageSize);
    }
}
