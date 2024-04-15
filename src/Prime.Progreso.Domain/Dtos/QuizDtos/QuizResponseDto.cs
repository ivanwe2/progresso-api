using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.QuizDtos
{
    public class QuizResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public int Duration { get; set; }

        public List<Guid> QuestionIds { get; set; }
    }
}
