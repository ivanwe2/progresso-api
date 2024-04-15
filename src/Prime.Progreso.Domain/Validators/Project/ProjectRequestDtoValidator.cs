using FluentValidation;
using Prime.Progreso.Domain.Dtos.Projects;

namespace Prime.Progreso.Domain.Validators.Project
{
    public class ProjectRequestDtoValidator : AbstractValidator<ProjectRequestDto>
    {
        public ProjectRequestDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(x => x.Milestones).NotEmpty();
        }
    }
}
