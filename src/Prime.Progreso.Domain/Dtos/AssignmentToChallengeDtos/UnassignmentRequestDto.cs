namespace Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos
{
    public class UnassignmentRequestDto
    {
        public int InternId { get; set; }

        public Guid CodingChallengeId { get; set; }

        public DateTime EndTime { get; set; }
    }
}
