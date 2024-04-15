namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IQuestionCategoryRepository : IBaseRepository
    {
        Task<bool> DoAllCategoriesExistAsync(List<Guid> categoryIds);
    }
}
