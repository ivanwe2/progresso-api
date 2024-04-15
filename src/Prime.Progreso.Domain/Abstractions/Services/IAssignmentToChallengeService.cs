using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IAssignmentToChallengeService
    {
        Task<AssignmentResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<AssignmentResponseDto>> GetPageAsync(int pageNumber,int pageSize);
        Task<AssignmentResponseDto> AssignInternAsync(AssignmentRequestDto dto);
        Task UnassignInternAsync(UnassignmentRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
