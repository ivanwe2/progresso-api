using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.Projects;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators;
using IValidatorFactory = Prime.Progreso.Domain.Abstractions.Factories.IValidatorFactory;

namespace Prime.Progreso.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMilestoneRepository _milestoneRepository;
        private IValidationProvider _validationProvider;

        public ProjectService(IProjectRepository projectRepository, IMilestoneRepository milestoneRepository,
            IValidationProvider validationProvider)
        {
            _projectRepository = projectRepository;
            _milestoneRepository = milestoneRepository;
            _validationProvider = validationProvider;
        }

        public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!_milestoneRepository.DoAllMilestonesExist(dto.Milestones))
            {
                throw new NotFoundException($"One or more milestone/s ids were not found!");
            }

            ProjectResponseDto createdProject = await _projectRepository.CreateAsync(dto);

            return createdProject;
        }

        public async Task UpdateAsync(Guid id, ProjectRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!_milestoneRepository.DoAllMilestonesExist(dto.Milestones))
            {
                throw new NotFoundException($"One or more milestone/s ids were not found!");
            }

            await _projectRepository.UpdateAsync(id, dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        public async Task<ProjectResponseDto> GetByIdAsync(Guid id)
        {
            ProjectResponseDto project = await _projectRepository.GetByIdAsync(id);

            return project;
        }

        public async Task<PaginatedResult<ProjectResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _projectRepository.GetPageAsync<ProjectResponseDto>(pageNumber, pageSize);
        }
    }
}
