using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;

namespace Prime.Progreso.Data.Repositories
{
    public class QuestionCategoryRepository : BaseRepository<QuestionCategory>, IQuestionCategoryRepository
    {
        public QuestionCategoryRepository(ProgresoDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public async Task<bool> DoAllCategoriesExistAsync(List<Guid> categoryIds)
        {
            var count = await Items.Where(e => categoryIds.Contains(e.Id)).CountAsync();

            return count == categoryIds.Count;
        }
    }
}
