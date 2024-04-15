using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.TestCase;

namespace Prime.Progreso.Data.Repositories
{
    public class TestCaseRepository : BaseRepository<TestCase>, ITestCaseRepository
    {
        public TestCaseRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<PaginatedResult<TestCaseResponseDto>> GetPageAsync(TestCasesPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.codingChallengeIds.Any())
            {
                query = FilterByTechnology(query, pagingInfo.codingChallengeIds);
            }

            return await Mapper.ProjectTo<TestCaseResponseDto>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        private IQueryable<TestCase> FilterByTechnology(IQueryable<TestCase> query, List<Guid> codingChallengeIdsFilter)
        {
            return query.Where(tc => codingChallengeIdsFilter.Contains(tc.CodingChallengeId));
        }
    }
}
