using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Dtos.QuizDtos
{
    public class QuizzesStatisticsPaginatedDto
    {
        public PaginatedResult<QuizStatisticsResponseDto> Quizzes { get; set; }
        public double CompletionRate { get; set; }
    }
}
