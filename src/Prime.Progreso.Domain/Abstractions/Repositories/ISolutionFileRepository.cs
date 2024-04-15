namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ISolutionFileRepository : IFileRepository
    {
        Task<string> CreateOrUpdateSolutionFile(Guid codingChallengeId, int userId, string code);
    }
}
