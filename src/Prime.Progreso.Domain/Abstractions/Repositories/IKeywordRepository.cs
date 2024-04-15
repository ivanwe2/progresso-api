using Prime.Progreso.Domain.Dtos.KeywordDtos;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IKeywordRepository : IBaseRepository
    {
        Task<RandomKeywordResponseDto> GetRandomKeywordAsync(RandomKeywordRequestDto dto);
        Task<bool> DoAllKeywordsExist(List<Guid> keywordIds);
    }
}
