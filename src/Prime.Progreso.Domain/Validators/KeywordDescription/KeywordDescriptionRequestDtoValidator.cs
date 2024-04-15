using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;

namespace Prime.Progreso.Domain.Validators.KeywordDescription
{
    public class KeywordDescriptionRequestDtoValidator : AbstractValidator<KeywordDescriptionRequestDto>
    {
        public KeywordDescriptionRequestDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty()
                                       .MaximumLength(300);
            RuleFor(x => x.KeywordId).NotEmpty();

            RuleFor(x => x.Difficulty).IsInEnum();
        }
    }
}
