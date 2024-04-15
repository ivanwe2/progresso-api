using FluentValidation;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;

namespace Prime.Progreso.Domain.Validators.TestCase
{
    public class TestCaseRequestDtoValidator : AbstractValidator<TestCaseRequestDto>
    {
        public TestCaseRequestDtoValidator()
        {
            RuleFor(x => x.CodingChallengeId).NotEmpty();
            RuleFor(x => x.InputData).NotEmpty();
            RuleFor(x => x.ExpectedOutput).NotEmpty();
        }
    }
}
