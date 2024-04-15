using FluentValidation;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;

namespace Prime.Progreso.Domain.Validators.Technology
{
    public class TechnologyRequestDtoValidator : AbstractValidator<TechnologyRequestDto>
    {
        public TechnologyRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }
}
