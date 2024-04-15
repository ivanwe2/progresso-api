using Microsoft.Extensions.Options;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.PathDtos;

namespace Prime.Progreso.Data.Repositories
{
    public class SolutionFileRepository : FileRepository<PathToSolutionDirectory>, ISolutionFileRepository
    {
        public SolutionFileRepository(IOptions<PathToSolutionDirectory> pathOptions)
            : base(pathOptions)
        {
        }

        public async Task<string> CreateOrUpdateSolutionFile(Guid codingChallengeId, int userId, string code)
        {
            string fileName = string.Format($"{codingChallengeId}_{userId}.txt");

            string path = string.Format($"{_directoryPath}\\{fileName}");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Dispose();

            using (var tw = new StreamWriter(path))
            {
                await tw.WriteLineAsync(code);
            }

            return path;
        }
    }
}
