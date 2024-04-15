using FluentValidation;
using Prime.Progreso.Domain.Dtos.Milestones;

namespace Prime.Progreso.Domain.Validators.Milestone
{
    public class MilestoneRequestDtoValidator : AbstractValidator<MilestoneRequestDto>
    {
        public MilestoneRequestDtoValidator()
        {
            RuleFor(x => x.Order).NotEmpty()
                                    .InclusiveBetween(0, int.MaxValue)
                                    .WithMessage("Order should be a positive number!");

            RuleFor(x => x.Duration).NotEmpty()
                                    .InclusiveBetween(0, int.MaxValue)
                                    .WithMessage("Duration should be a positive number!");
        }
    }
}
