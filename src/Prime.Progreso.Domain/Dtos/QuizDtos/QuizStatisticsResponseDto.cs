using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;

namespace Prime.Progreso.Domain.Dtos.QuizDtos
{
    public class QuizStatisticsResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public List<QuestionStatisticsResponseDto> Questions { get; set; }

        public double CompletionRate { get; set; }
    }
}
