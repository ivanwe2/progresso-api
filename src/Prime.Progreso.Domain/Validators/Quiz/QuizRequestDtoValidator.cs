using FluentValidation;
using Prime.Progreso.Domain.Dtos.QuizDtos;

namespace Prime.Progreso.Domain.Validators.Quiz
{
    public class QuizRequestDtoValidator : AbstractValidator<QuizRequestDto>
    {
        public QuizRequestDtoValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

            RuleFor(x => x.Duration).NotEmpty()
                                    .InclusiveBetween(1, int.MaxValue)
                                    .WithMessage("Duration should be a positive number, higher than 1.");

            RuleFor(x => x.QuestionIds).NotEmpty()
                                       .Must(ContainAtLeastTwoQuestions)
                                       .WithMessage("Quiz must contain at least two questions.");
        }

        private bool ContainAtLeastTwoQuestions(List<Guid> questionIds)
        {
            return questionIds.Count > 1;
        }
    }
}
