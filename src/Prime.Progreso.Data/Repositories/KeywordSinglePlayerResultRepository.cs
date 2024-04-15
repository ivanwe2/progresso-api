using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Data.Extensions;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordSinglePlayerResultRepository : BaseRepository<KeywordSinglePlayerResult>, IKeywordSinglePlayerResultRepository
    {
        public KeywordSinglePlayerResultRepository(ProgresoDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
        {
        }

        public async Task<PaginatedResult<KeywordSinglePlayerResultResponseDto>> GetPageAsync(
            KeywordSinglePlayerResultPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.UserIds.Any())
            {
                query = query.Where(x => pagingInfo.UserIds.Contains(x.UserId));
            }

            if (pagingInfo.KeywordIds.Any())
            {
                query = query.Where(x => pagingInfo.KeywordIds.Contains(x.KeywordId));
            }

            if (pagingInfo.LanguageIds.Any())
            {
                query = query.Where(x => pagingInfo.LanguageIds.Contains(x.Keyword.LanguageId));
            }

            return await Mapper
                .ProjectTo<KeywordSinglePlayerResultResponseDto>(query)
                .PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }
    }
}
