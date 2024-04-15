namespace Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult
{
    public class KeywordSinglePlayerResultPagingInfo : PagingInfo
    {
        public List<int> UserIds { get; set; } = new List<int>();

        public List<Guid> KeywordIds { get; set; } = new List<Guid>();

        public List<Guid> LanguageIds { get; set; } = new List<Guid>();
    }
}
