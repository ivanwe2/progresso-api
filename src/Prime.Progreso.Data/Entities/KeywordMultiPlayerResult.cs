using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class KeywordMultiPlayerResult : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public Guid KeywordId { get; set; }

        public Keyword Keyword { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
    }
}
