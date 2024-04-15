namespace Prime.Progreso.Domain.Dtos.TestCaseDtos
{
    public class TestCaseRequestDto
    {
        public Guid CodingChallengeId { get; set; }

        public string InputData { get; set; }

        public string ExpectedOutput { get; set; }
    }
}
