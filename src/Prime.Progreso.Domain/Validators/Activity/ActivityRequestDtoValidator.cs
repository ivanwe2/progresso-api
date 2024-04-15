using FluentValidation;
using Prime.Progreso.Domain.Dtos.ActivityDtos;

namespace Prime.Progreso.Domain.Validators.Activity
{
    public class ActivityRequestDtoValidator : AbstractValidator<ActivityRequestDto>
    {
        public ActivityRequestDtoValidator()
        {
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
