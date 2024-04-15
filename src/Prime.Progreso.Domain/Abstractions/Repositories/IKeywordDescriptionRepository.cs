using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;
using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IKeywordDescriptionRepository : IBaseRepository
    {
        Task<bool> CheckIfAnswerIsCorrect(Guid id, string answer);
        Task<RandomKeywordDescriptionResponseDto> GetRandomAsync(Guid languageId, List<Difficulty> difficultyLevels);
    }
}
