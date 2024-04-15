using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos
{
    public class KeywordDescriptionSinglePlayerResultResponseDto : BaseResponseDto
    {
        public int UserId { get; set; }

        public Guid KeywordDescriptionId { get; set; }

        public string Answer { get; set; }

        public bool IsCorrect { get; set; }
    }
}
