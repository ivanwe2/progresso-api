using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordDescriptionMultiPlayerResultRepository
        : BaseRepository<KeywordDescriptionMultiPlayerResult>, IKeywordDescriptionMultiPlayerResultRepository
    {
        public KeywordDescriptionMultiPlayerResultRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }
    }
}
