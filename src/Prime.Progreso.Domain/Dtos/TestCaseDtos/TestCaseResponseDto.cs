using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.TestCaseDtos
{
    public class TestCaseResponseDto : BaseResponseDto
    {
        public Guid CodingChallengeId { get; set; }

        public string InputData { get; set; }

        public string ExpectedOutput { get; set; }
    }
}
