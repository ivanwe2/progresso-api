using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Quiz;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IQuizService
    {
        Task<QuizResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<QuizResponseDto>> GetPageAsync(QuizesPagingInfo pagingInfo);
        Task<QuizResponseDto> CreateAsync(QuizRequestDto dto);
        Task UpdateAsync(Guid id, QuizRequestDto dto);
        Task DeleteAsync(Guid id);
        Task<QuizStatisticsResponseDto> GetStatisticsByIdAsync(Guid id);
        Task<QuizzesStatisticsPaginatedDto> GetPagedStatisticsAsync(QuizStatisticsPagingInfo pagingInfo);
    }
}
