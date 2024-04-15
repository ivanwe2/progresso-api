namespace Prime.Progreso.Data.Entities
{
    public class QuizQuestionLink
    {
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
