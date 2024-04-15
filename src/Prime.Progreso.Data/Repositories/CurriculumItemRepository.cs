using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;

namespace Prime.Progreso.Data.Repositories
{
    public class CurriculumItemRepository : BaseRepository<CurriculumItem>, ICurriculumItemRepository
    {
        public CurriculumItemRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }
    }
}
