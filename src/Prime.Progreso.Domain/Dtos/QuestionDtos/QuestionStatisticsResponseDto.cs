using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.QuestionDtos
{
    public class QuestionStatisticsResponseDto : BaseResponseDto
    {
        public string Title { get; set; }
        public List<AnswerStatisticsResponseDto> Answers { get; set; }
        public double CompletionRate { get; set; }
    }
}
