using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Pagination.Quiz;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IQuizRepository : IBaseRepository
    {
        Task UpdateAsync(Guid id, QuizRequestDto dto);
        Task<QuizResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<QuizResponseDto>> GetPageByFilterAsync(QuizesPagingInfo pagingInfo);
        Task<bool> IsQuestionRelatedToQuizAsync(Guid id, Guid questionId);
    }
}
