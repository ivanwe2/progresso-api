using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class Language : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public List<Keyword> Keywords { get; set; }
    }
}
