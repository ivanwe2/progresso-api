namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos
{
    public class KeywordDescriptionMultiPlayerResultRequestDto
    {
        public int UserId { get; set; }

        public Guid KeywordDescriptionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
