using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class QuestionCategory : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
