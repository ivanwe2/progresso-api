using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.AnswerDtos
{
    public class AnswerStatisticsResponseDto : BaseResponseDto
    {
        public string Content { get; set; }
        public double ChoiceRate { get; set; }
    }
}
