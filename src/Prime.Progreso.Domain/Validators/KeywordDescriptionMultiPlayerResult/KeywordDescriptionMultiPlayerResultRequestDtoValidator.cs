using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;

namespace Prime.Progreso.Domain.Validators.KeywordDescriptionMultiPlayerResult
{
    public class KeywordDescriptionMultiPlayerResultRequestDtoValidator
        : AbstractValidator<KeywordDescriptionMultiPlayerResultRequestDto>
    {
        public KeywordDescriptionMultiPlayerResultRequestDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.KeywordDescriptionId).NotEmpty();
            RuleFor(x => x.IsCorrect).Must(x => x == false || x == true);
        }
    }
}
