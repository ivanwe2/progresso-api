using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Quiz;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IQuizExecutionRepository : IBaseRepository
    {
        Task<QuizExecutionResponseDto> GetByIdAsync(Guid id, int userId);
        Task<PaginatedResult<QuizExecutionResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<bool> IsRelatedToUserAsync(Guid id, int userId);
        Task<List<QuizStatisticsResponseDto>> GetAllQuizStatisticsAsync(QuizStatisticsPagingInfo pagingInfo);
    }
}
