using FluentValidation;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Validators.Keyword
{
    public class KeywordRequestDtoValidator : AbstractValidator<KeywordRequestDto>
    {
        public KeywordRequestDtoValidator() 
        {
            RuleFor(x => x.Word).NotEmpty()
                                       .MaximumLength(20);
            RuleFor(x => x.LanguageId).NotEmpty();
        } 
    }
}
