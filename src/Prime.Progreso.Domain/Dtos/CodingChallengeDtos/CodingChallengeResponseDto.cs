using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.CodingChallengeDtos
{
    public class CodingChallengeResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Codebase { get; set; }

        public CodingChallengeType Type { get; set; }

        public TechnologyResponseDto Technology { get; set; }
    }
}
