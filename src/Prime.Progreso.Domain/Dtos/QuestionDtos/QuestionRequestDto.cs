using Prime.Progreso.Domain.Dtos.AnswerDtos;

namespace Prime.Progreso.Domain.Dtos.QuestionDtos
{
    public class QuestionRequestDto
    {
        public string Title { get; set; }

        public List<Guid> QuestionCategoryIds { get; set; }

        public List<AnswerRequestDto> Answers { get; set; }
    }
}
