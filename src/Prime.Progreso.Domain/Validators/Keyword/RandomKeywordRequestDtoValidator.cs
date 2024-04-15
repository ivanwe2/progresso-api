using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordDtos;

namespace Prime.Progreso.Domain.Validators.Keyword
{
    public class RandomKeywordRequestDtoValidator : AbstractValidator<RandomKeywordRequestDto>
    {
        public RandomKeywordRequestDtoValidator()
        {
            RuleForEach(x => x.Difficulties).NotEmpty()
                                            .IsInEnum();

            RuleFor(x => x.LanguageId).NotEmpty();
        }
    }
}
