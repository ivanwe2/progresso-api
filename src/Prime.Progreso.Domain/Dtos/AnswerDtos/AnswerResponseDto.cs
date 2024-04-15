using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.AnswerDtos
{
    public class AnswerResponseDto : BaseResponseDto
    {
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}
