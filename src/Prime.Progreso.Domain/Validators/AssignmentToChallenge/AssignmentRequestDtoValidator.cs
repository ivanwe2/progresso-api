using FluentValidation;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;

namespace Prime.Progreso.Domain.Validators.AssignmentToChallenge
{
    public class AssignmentRequestDtoValidator : AbstractValidator<AssignmentRequestDto>
    {
        public AssignmentRequestDtoValidator() 
        {
            RuleFor(x => x.InternId).NotEmpty();

            RuleFor(x => x.CodingChallengeId).NotEmpty();

            RuleFor(x => x.StartTime).NotEmpty();

            RuleFor(x => x.EndTime)
                .GreaterThanOrEqualTo(x => x.StartTime).WithMessage("End time cannot be less than the start time!");
        }
    }
}
