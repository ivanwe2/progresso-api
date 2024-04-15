using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.KeywordDtos;

namespace Prime.Progreso.Domain.Dtos.LanguageDtos
{
    public class LanguageResponseDto : BaseResponseDto
    {
        public string Name { get; set; }

        public List<KeywordResponseDto> Keywords { get; set; }
    }
}
