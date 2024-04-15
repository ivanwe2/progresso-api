using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos
{
    public class AssignmentResponseDto : BaseResponseDto
    {
        public int InternId { get; set; }

        public Guid CodingChallengeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
