using FluentValidation;
using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;

namespace Prime.Progreso.Domain.Validators.QuestionCategory
{
    public class QuestionCategoryRequestDtoValidator : AbstractValidator<QuestionCategoryRequestDto>
    {
        public QuestionCategoryRequestDtoValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(50); ;
            RuleFor(x => x.Description).MaximumLength(250);
        }
    }
}
