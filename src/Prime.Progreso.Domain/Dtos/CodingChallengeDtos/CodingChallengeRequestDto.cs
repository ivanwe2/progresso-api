using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.CodingChallengeDtos
{
    public class CodingChallengeRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Codebase { get; set; }

        public CodingChallengeType Type { get; set; }

        public Guid TechnologyId { get; set; }
    }
}
