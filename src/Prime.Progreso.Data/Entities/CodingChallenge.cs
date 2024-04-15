using Prime.Progreso.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class CodingChallenge : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Codebase { get; set; }

        [Required]
        public CodingChallengeType Type { get; set; }

        [Required]
        public Guid? TechnologyId { get; set; }

        public Technology Technology { get; set; }
    }
}
