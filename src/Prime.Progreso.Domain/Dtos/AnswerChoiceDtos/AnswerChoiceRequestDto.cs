namespace Prime.Progreso.Domain.Dtos.AnswerChoiceDtos
{
    public class AnswerChoiceRequestDto
    {
        public Guid QuizExecutionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
    }
}
