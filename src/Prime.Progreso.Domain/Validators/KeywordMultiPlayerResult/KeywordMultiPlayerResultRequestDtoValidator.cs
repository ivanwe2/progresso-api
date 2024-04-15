using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;

namespace Prime.Progreso.Domain.Validators.KeywordMultiPlayerResult
{
    public class KeywordMultiPlayerResultRequestDtoValidator 
        : AbstractValidator<KeywordMultiPlayerResultRequestDto>
    {
        public KeywordMultiPlayerResultRequestDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.KeywordId).NotEmpty();
        }
    }
}
