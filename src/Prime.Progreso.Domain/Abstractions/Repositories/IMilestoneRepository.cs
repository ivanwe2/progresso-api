namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IMilestoneRepository : IBaseRepository
    {
        bool DoAllMilestonesExist(List<Guid> milestones);
    }
}
