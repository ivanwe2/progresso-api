using FluentValidation;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;

namespace Prime.Progreso.Domain.Validators.Curriculum
{
    public class CurriculumRequestDtoValidator : AbstractValidator<CurriculumRequestDto>
    {
        public CurriculumRequestDtoValidator()
        {
            RuleFor(x => x.TechnologyId).NotEmpty();

            RuleFor(x => x.Description).MaximumLength(250);

            RuleFor(x => x.Duration)
                .NotEmpty()
                .InclusiveBetween(1, 366);
        }
    }
}
