using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordDescriptionMultiPlayerResultService
    {
        Task<KeywordDescriptionMultiPlayerResultResponseDto> CreateAsync(
            KeywordDescriptionMultiPlayerResultRequestDto dto);
    }
}
