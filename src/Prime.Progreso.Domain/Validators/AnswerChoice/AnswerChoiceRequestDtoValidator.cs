using FluentValidation;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;

namespace Prime.Progreso.Domain.Validators.AnswerChoice
{
    public class AnswerChoiceRequestDtoValidator : AbstractValidator<AnswerChoiceRequestDto>
    {
        public AnswerChoiceRequestDtoValidator()
        {
            RuleFor(x => x.QuizExecutionId).NotEmpty();
            RuleFor(x => x.QuestionId).NotEmpty();
            RuleFor(x => x.ChoiceId).NotEmpty();
        }
    }
}
