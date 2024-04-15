using FluentValidation;
using Prime.Progreso.Domain.Dtos.LanguageDtos;

namespace Prime.Progreso.Domain.Validators.Language
{
    public class LanguageRequestDtoValidator : AbstractValidator<LanguageRequestDto>
    {
        public LanguageRequestDtoValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }
}
