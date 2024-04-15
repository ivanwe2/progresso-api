using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.RequestForms.RandomKeywordDescription
{
    public class RandomKeywordDescriptionGetRequestForm
    {
        public Guid LanguageId { get; set; }

        public List<Difficulty> DifficultyLevels { get; set; }
    }
}
