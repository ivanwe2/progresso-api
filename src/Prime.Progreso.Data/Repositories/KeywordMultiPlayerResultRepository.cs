using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordMultiPlayerResultRepository 
        : BaseRepository<KeywordMultiPlayerResult>, IKeywordMultiPlayerResultRepository
    {
        public KeywordMultiPlayerResultRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }
    }
}
