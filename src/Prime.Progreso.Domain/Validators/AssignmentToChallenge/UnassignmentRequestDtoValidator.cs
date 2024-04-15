using FluentValidation;
using FluentValidation.AspNetCore;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;

namespace Prime.Progreso.Domain.Validators.AssignmentToChallenge
{
    public class UnassignmentRequestDtoValidator : AbstractValidator<UnassignmentRequestDto>
    {
        public UnassignmentRequestDtoValidator() 
        { 
            RuleFor(x => x.InternId).NotEmpty();

            RuleFor(x => x.CodingChallengeId).NotEmpty();

            RuleFor(x => x.EndTime).NotEmpty();
        }
    }
}
