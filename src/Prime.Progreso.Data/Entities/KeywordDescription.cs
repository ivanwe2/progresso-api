using Prime.Progreso.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class KeywordDescription : BaseEntity
    {
        [Required]
        public string Description { get; set; }

        public Difficulty Difficulty { get; set; }

        public Guid KeywordId { get; set; }

        public Keyword Keyword { get; set; }
    }
}
