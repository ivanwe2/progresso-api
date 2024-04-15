using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos
{
    public class KeywordDescriptionMultiPlayerResultResponseDto : BaseResponseDto
    {
        public int UserId { get; set; }

        public Guid KeywordDescriptionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
