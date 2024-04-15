using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.CodingChallenge;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ICodingChallengeService
    {
        Task<CodingChallengeResponseDto> CreateAsync(CodingChallengeRequestDto dto);
        Task UpdateAsync(Guid id, CodingChallengeRequestDto dto);
        Task DeleteAsync(Guid id);
        Task<CodingChallengeResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<CodingChallengeResponseDto>> GetPageAsync(CodingChallengesPagingInfo pagingInfo);
    }
}
