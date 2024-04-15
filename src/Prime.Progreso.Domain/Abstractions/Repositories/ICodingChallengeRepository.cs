using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.CodingChallenge;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface ICodingChallengeRepository : IBaseRepository
    {
        Task<bool> HasAnyRelatedToTechnologyAsync(Guid technologyId);
        Task<PaginatedResult<CodingChallengeResponseDto>> GetPageAsync(CodingChallengesPagingInfo pagingInfo);
    }
}
