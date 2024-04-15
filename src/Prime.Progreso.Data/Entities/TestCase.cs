using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class TestCase : BaseEntity
    {
        [Required]
        public Guid CodingChallengeId { get; set; }

        public CodingChallenge CodingChallenge { get; set; }

        [Required]
        public string InputData { get; set; }

        [Required]
        public string ExpectedOutput { get; set; }
    }
}
