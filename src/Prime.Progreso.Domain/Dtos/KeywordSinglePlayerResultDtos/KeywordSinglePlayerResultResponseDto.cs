using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos
{
    public class KeywordSinglePlayerResultResponseDto : BaseResponseDto
    {
        public int UserId { get; set; }

        public Guid KeywordId { get; set; }

        public string Answer { get; set; }

        public bool? IsCorrect { get; set; }
    }
}
