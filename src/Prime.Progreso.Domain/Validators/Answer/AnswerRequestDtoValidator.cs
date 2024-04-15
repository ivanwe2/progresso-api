using FluentValidation;
using Prime.Progreso.Domain.Dtos.AnswerDtos;

namespace Prime.Progreso.Domain.Validators.Answer
{
    public class AnswerRequestDtoValidator : AbstractValidator<AnswerRequestDto>
    {
        public AnswerRequestDtoValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
            RuleFor(x => x.IsCorrect).NotNull();
        }
    }
}
