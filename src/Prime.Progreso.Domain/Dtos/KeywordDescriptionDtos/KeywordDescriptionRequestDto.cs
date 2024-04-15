using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos
{
    public class KeywordDescriptionRequestDto
    {
        public string Description { get; set; }

        public Difficulty Difficulty { get; set; }

        public Guid KeywordId { get; set; }
    }
}
