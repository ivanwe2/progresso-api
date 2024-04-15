using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.Projects;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public List<Milestone> GetMilestones(List<Guid> milestoneGuids)
        {
            return DbContext.Milestones.Where(e => milestoneGuids.Contains(e.Id)).ToList();
        }

        public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto)
        {
            var entity = Mapper.Map<Project>(dto);
            entity.Milestones = GetMilestones(dto.Milestones);

            await Items.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return Mapper.Map<ProjectResponseDto>(entity);
        }

        public async Task UpdateAsync(Guid id, ProjectRequestDto dto)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Project was not found!");
            }

            Mapper.Map(dto, entity);
            entity.Milestones = GetMilestones(dto.Milestones);

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task<ProjectResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await Items
                .Include(c => c.Milestones)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Project was not found!");
            }

            return Mapper.Map<ProjectResponseDto>(entity);
        }
    }
}
