using FluentValidation;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using System.Text.RegularExpressions;

namespace Prime.Progreso.Domain.Validators.CodingChallenge
{
    public class CodingChallengeRequestDtoValidator : AbstractValidator<CodingChallengeRequestDto>
    {
        Regex urlRegex = new Regex("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_\\+.~#?&\\/\\/=]*)", RegexOptions.None, TimeSpan.FromSeconds(2));
        public CodingChallengeRequestDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.Codebase).NotEmpty()
                                    .Matches(urlRegex)
                                    .WithMessage("Codebase should be a valid URL!");

            RuleFor(x => x.Type).NotEmpty();

            RuleFor(x => x.TechnologyId).NotEmpty();
        }
    }
}
