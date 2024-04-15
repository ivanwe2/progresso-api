using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;

namespace Prime.Progreso.Domain.Validators.KeywordSinglePlayerResult
{
    public class KeywordSinglePlayerResultIsCorrectUpdateRequestDtoValidator : AbstractValidator<KeywordSinglePlayerResultIsCorrectUpdateRequestDto>
    {
        public KeywordSinglePlayerResultIsCorrectUpdateRequestDtoValidator()
        {
            RuleFor(x => x.IsCorrect).NotEmpty();
        }
    }
}