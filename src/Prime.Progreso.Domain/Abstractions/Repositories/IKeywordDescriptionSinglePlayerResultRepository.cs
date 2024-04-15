using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IKeywordDescriptionSinglePlayerResultRepository : IBaseRepository
    {
        Task<KeywordDescriptionSinglePlayerResultResponseDto> CreateAsync(
            KeywordDescriptionSinglePlayerResultWithIsCorrectDto dto);
        Task UpdateAsync(Guid id, KeywordDescriptionSinglePlayerResultWithIsCorrectDto dto);
    }
}
