using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.QuestionDtos
{
    public class QuestionResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public List<Guid> QuestionCategoryIds { get; set; }

        public List<AnswerResponseDto> Answers { get; set; }
    }
}
