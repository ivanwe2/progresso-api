using FluentValidation;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;

namespace Prime.Progreso.Domain.Validators.CurriculumItem
{
    public class CurriculumItemRequestDtoValidator : AbstractValidator<CurriculumItemRequestDto>
    {
        public CurriculumItemRequestDtoValidator()
        {
            RuleFor(x => x.ActivityId).NotEmpty();

            RuleFor(x => x.CurriculumId).NotEmpty();

            RuleFor(x => x.DayOfInternship)
                .NotEmpty()
                .InclusiveBetween(1, int.MaxValue);
        }
    }
}
