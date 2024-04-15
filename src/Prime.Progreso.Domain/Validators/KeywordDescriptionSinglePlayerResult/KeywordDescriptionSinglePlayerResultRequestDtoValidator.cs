using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;

namespace Prime.Progreso.Domain.Validators.KeywordDescriptionSinglePlayerResult
{
    public class KeywordDescriptionSinglePlayerResultRequestDtoValidator
        : AbstractValidator<KeywordDescriptionSinglePlayerResultRequestDto>
    {
        public KeywordDescriptionSinglePlayerResultRequestDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.KeywordDescriptionId).NotEmpty();
            RuleFor(x => x.Answer).NotEmpty();
        }
    }
}
