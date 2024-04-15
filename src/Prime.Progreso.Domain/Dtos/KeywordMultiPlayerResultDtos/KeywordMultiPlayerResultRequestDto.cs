namespace Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos
{
    public class KeywordMultiPlayerResultRequestDto
    {
        public int UserId { get; set; }

        public Guid KeywordId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
