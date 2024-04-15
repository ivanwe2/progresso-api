using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.KeywordDtos
{
    public class RandomKeywordRequestDto
    {
        public Guid LanguageId { get; set; }

        public List<Difficulty> Difficulties { get; set; }
    }
}
