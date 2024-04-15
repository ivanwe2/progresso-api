using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Dtos.SolutionDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IAssignmentToChallengeRepository : IBaseRepository
    {
        Task<AssignmentResponseDto> GetByInternAndChallengeIdsAsync(int internId, Guid codingChallengeId);
        Task<AssignmentResponseDto> GetByIdAsync(Guid id, int userId);
        Task<PaginatedResult<AssignmentResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize,
                                                                                     int userId);
        Task UnassignInternAsync(UnassignmentRequestDto dto);
        Task<bool> HasAnyAsync(Guid codingChallengeId, int userId);
        Task<List<Guid>> GetAssignedCodingChallengeIdsAsync(int userId);
        Task<AssignmentResponseDto> AddOrUpdateAsync(AssignmentRequestDto dto);
        Task<SolutionResponseDto> UpdateSolutionPathAsync(int userId, SolutionRequestDto dto);
        Task<SolutionResponseDto> GetSolutionByInternAndChallengeIdsAsync(int internId, Guid codingChallengeId);
    }
}
