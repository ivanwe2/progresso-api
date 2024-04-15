using Prime.Progreso.Domain.Dtos.Projects;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IProjectRepository : IBaseRepository
    {
        Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto);

        Task UpdateAsync(Guid id, ProjectRequestDto dto);

        Task<ProjectResponseDto> GetByIdAsync(Guid id);
    }
}
