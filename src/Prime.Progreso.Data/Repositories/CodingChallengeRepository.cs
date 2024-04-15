using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.CodingChallenge;

namespace Prime.Progreso.Data.Repositories
{
    public class CodingChallengeRepository : BaseRepository<CodingChallenge>, ICodingChallengeRepository
    {
        public CodingChallengeRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<PaginatedResult<CodingChallengeResponseDto>> GetPageAsync(CodingChallengesPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.codingChallengeIds.Any())
            {
                query = FilterByTechnology(query, pagingInfo.codingChallengeIds);
            }

            return await Mapper.ProjectTo<CodingChallengeResponseDto>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task<bool> HasAnyRelatedToTechnologyAsync(Guid technologyId)
        {
            return await Items.AnyAsync(c => c.TechnologyId == technologyId);
        }

        private IQueryable<CodingChallenge> FilterByTechnology(IQueryable<CodingChallenge> query, List<Guid> codingChallengeIdsFilter)
        {
            return query.Where(tc => codingChallengeIdsFilter.Contains(tc.Id));
        }

    }
}
