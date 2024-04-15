using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class KeywordDescriptionMultiPlayerResult : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public Guid KeywordDescriptionId { get; set; }

        public KeywordDescription KeywordDescription { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
    }
}
