using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Question : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public List<QuestionCategory> QuestionCategories { get; set; } = new();

        public List<CategorizedQuestion> CategorizedQuestions { get; set; } = new();

        public List<Answer> Answers { get; set; } = new();
    }
}
