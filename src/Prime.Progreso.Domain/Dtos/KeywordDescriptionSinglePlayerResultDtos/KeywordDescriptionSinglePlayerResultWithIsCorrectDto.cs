namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos
{
    public class KeywordDescriptionSinglePlayerResultWithIsCorrectDto
    {
        public int UserId { get; set; }

        public Guid KeywordDescriptionId { get; set; }

        public string Answer { get; set; }

        public bool IsCorrect { get; set; }
    }
}
