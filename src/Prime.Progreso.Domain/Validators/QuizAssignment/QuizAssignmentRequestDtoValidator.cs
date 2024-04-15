using FluentValidation;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Validators.QuizAssignment
{
    public class QuizAssignmentRequestDtoValidator : AbstractValidator<QuizAssignmentRequestDto>
    {
        public QuizAssignmentRequestDtoValidator()
        {
            RuleFor(x => x.QuizId).NotEmpty();

            RuleFor(x => x.AssigneeId).NotEmpty();

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Start time cannot be before the current time!");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartTime)
                .WithMessage("End time cannot be before the Start time!");
        }
    }
}
