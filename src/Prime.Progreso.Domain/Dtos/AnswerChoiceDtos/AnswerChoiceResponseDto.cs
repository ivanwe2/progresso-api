using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.AnswerChoiceDtos
{
    public class AnswerChoiceResponseDto : BaseResponseDto
    {
        public Guid QuizExecutionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
    }
}
