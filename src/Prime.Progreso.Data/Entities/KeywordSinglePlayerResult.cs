using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class KeywordSinglePlayerResult : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public Guid KeywordId { get; set; }

        public Keyword Keyword { get; set; }

        [Required]
        public string Answer { get; set; }

        public bool? IsCorrect { get; set; }
    }
}
