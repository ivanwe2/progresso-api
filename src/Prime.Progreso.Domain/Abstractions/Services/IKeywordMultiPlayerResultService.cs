using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordMultiPlayerResultService
    {
        Task<KeywordMultiPlayerResultResponseDto> CreateAsync(KeywordMultiPlayerResultRequestDto dto);
    }
}
