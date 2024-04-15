using Prime.Progreso.Domain.Dtos.Projects;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto);

        Task UpdateAsync(Guid id, ProjectRequestDto dto);

        Task DeleteAsync(Guid id);

        Task<ProjectResponseDto> GetByIdAsync(Guid id);

        Task<PaginatedResult<ProjectResponseDto>> GetPageAsync(int pageNumber, int pageSize);
    }
}
