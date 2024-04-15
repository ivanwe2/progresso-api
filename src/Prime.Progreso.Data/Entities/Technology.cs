using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Technology : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
