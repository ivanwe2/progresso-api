using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public virtual List<Milestone> Milestones { get; set; }
    }
}
