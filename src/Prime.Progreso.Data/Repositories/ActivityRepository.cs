using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(ProgresoDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }
    }
}
