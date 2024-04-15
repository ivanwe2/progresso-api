namespace Prime.Progreso.Data.Entities
{
    public class CategorizedQuestion
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public Guid QuestionCategoryId { get; set; }
        public QuestionCategory QuestionCategory { get; set; } = null!;
    }
}
