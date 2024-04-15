using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos
{
    public class KeywordMultiPlayerResultResponseDto : BaseResponseDto
    {
        public int UserId { get; set; }

        public Guid KeywordId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
