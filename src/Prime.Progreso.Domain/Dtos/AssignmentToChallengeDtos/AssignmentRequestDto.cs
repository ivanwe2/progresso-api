namespace Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos
{
    public class AssignmentRequestDto
    {
        public int InternId { get; set; }

        public Guid CodingChallengeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
