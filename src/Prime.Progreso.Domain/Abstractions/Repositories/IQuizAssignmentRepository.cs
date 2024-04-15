using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IQuizAssignmentRepository : IBaseRepository
    {
        Task<TOutput> GetByIdAsync<TOutput>(Guid id, int userId);
        Task<PaginatedResult<QuizAssignmentResponseDto>> GetPageByFilterAsync(PagingInfo pagingInfo, int userId);
        Task<bool> IsInternAssignedToQuizAsync(Guid quizId, int userId);
        Task<List<Guid>> GetQuizIdsAssignedToUser(PagingInfo pagingInfo, int userId, DateTime timeOfRequestedAccess);
    }
}
