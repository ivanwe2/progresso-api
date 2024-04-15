namespace Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos
{
    public class KeywordSinglePlayerResultRequestDto
    {
        public int UserId { get; set; }

        public Guid KeywordId { get; set; }

        public string Answer { get; set; }
    }
}
