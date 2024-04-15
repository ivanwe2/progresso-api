namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ILanguageRepository : IBaseRepository
    {
        Task<bool> DoAllLanguagesExist(List<Guid> languageIds);
    }
}
