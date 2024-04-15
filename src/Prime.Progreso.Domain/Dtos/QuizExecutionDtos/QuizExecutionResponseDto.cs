using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.QuizExecutionDtos
{
    public class QuizExecutionResponseDto : BaseResponseDto
    {
        public Guid QuizId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
