using FluentValidation;
using Prime.Progreso.Domain.Dtos.SolutionDtos;

namespace Prime.Progreso.Domain.Validators.Solution
{
    public class SolutionRequestDtoValidator : AbstractValidator<SolutionRequestDto>
    {
        public SolutionRequestDtoValidator()
        {
            RuleFor(x => x.CodingChallengeId).NotEmpty();
        }
    }
}
