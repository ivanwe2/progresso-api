namespace Prime.Progreso.Domain.Dtos.QuizDtos
{
    public class QuizRequestDto
    {
        public string Title { get; set; }

        public int Duration { get; set; }

        public List<Guid> QuestionIds { get; set; }
    }
}
