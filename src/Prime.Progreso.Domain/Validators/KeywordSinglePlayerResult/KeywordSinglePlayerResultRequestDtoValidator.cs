using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;

namespace Prime.Progreso.Domain.Validators.KeywordSinglePlayerResult
{
    public class KeywordSinglePlayerResultRequestDtoValidator : AbstractValidator<KeywordSinglePlayerResultRequestDto>
    {
        public KeywordSinglePlayerResultRequestDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.KeywordId).NotEmpty();

            RuleFor(x => x.Answer).NotEmpty()
                                  .MaximumLength(300);
        }
    }
}
