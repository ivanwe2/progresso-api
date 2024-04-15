namespace Prime.Progreso.Domain.Pagination.Quiz
{
    public class QuizStatisticsPagingInfo : PagingInfo
    {
        public List<int> userIds { get; set; } = new();
        public List<int> seasonIds { get; set; } = new();
    }
}
