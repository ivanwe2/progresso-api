using FluentValidation;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;

namespace Prime.Progreso.Domain.Validators.QuizExecution
{
    public class QuizExecutionRequestDtoValidator : AbstractValidator<QuizExecutionRequestDto>
    {
        public QuizExecutionRequestDtoValidator()
        {
            RuleFor(x => x.QuizId).NotEmpty();

            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.StartTime).NotEmpty();

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartTime).WithMessage("End time cannot be less than the start time!");
        }
    }
}
