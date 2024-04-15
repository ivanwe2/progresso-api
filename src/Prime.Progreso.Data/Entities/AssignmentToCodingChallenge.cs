using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class AssignmentToCodingChallenge : BaseEntity
    {
        [Required]
        public int InternId { get; set; }

        [Required]
        public Guid CodingChallengeId { get; set; }

        public CodingChallenge CodingChallenge { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public string SolutionPath { get; set; }
    }
}
