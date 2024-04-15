using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class LanguageRepository : BaseRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public override async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var entity = await Items
                                .AsNoTracking()
                                .Include(l => l.Keywords)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TOutput).Name} was not found!");
            }
            return Mapper.Map<TOutput>(entity);
        }

        public async Task<bool> DoAllLanguagesExist(List<Guid> languageIds)
        {
            var count = await Items.Where(e => languageIds.Contains(e.Id)).CountAsync();

            return count == languageIds.Count;
        }
    }
}
