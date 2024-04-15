using FluentValidation;
using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Validators.Answer;

namespace Prime.Progreso.Domain.Validators.Question
{
    public class QuestionRequestDtoValidator : AbstractValidator<QuestionRequestDto>
    {
        public QuestionRequestDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

            RuleFor(x => x.QuestionCategoryIds).NotEmpty();

            RuleForEach(x => x.Answers).SetValidator(new AnswerRequestDtoValidator()).NotEmpty();

            RuleFor(x => x.Answers).Must(ContainExactlyFourAnswers)
                                   .WithMessage("A question should be related to exactly 4 answers.");

            RuleFor(x => x.Answers).Must(HaveOneCorrectAnswer)
                                   .WithMessage("A question should have exactly 1 correct answer.");
        }
        private bool ContainExactlyFourAnswers(List<AnswerRequestDto> answers)
        {
            return answers != null && answers.Count == 4;
        }

        private bool HaveOneCorrectAnswer(List<AnswerRequestDto> answers)
        {
            return answers != null && answers.Count(a => a.IsCorrect) == 1;
        }
    }
}
