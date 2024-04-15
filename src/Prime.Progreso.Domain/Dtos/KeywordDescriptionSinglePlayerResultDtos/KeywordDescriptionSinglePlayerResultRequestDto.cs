namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos
{
    public class KeywordDescriptionSinglePlayerResultRequestDto
    {
        public int UserId { get; set; }

        public Guid KeywordDescriptionId { get; set; }

        public string Answer { get; set; }
    }
}
