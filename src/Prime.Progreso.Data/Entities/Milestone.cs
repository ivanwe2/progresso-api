using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Milestone : BaseEntity
    {
        public string Description { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }
    }
}
