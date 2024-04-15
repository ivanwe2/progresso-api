using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Quiz : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Duration { get; set; }

        public List<Question> Questions { get; set; } = new();

        public List<QuizQuestionLink> QuizQuestionLinks { get; set; } = new();
    }
}
