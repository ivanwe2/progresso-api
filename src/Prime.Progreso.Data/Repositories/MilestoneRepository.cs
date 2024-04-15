using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;

namespace Prime.Progreso.Data.Repositories
{
    public class MilestoneRepository : BaseRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        } 

        public bool DoAllMilestonesExist(List<Guid> milestones)
        {
            var milestoneCount = DbContext.Milestones.Where(e => milestones.Contains(e.Id)).Count();

            if (milestoneCount != milestones.Count())
            {
                return false;
            }

            return true;
        }
    }
}
