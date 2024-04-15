using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Answer : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
    }
}
