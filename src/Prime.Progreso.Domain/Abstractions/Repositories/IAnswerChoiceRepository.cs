using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IAnswerChoiceRepository : IBaseRepository
    {
        Task<AnswerChoiceResponseDto> GetByIdAsync(Guid id, int userId);
        Task<PaginatedResult<AnswerChoiceResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<List<QuestionStatisticsResponseDto>> GetStatisticsByQuizAndQuestionIdsAsync(Guid quizId, List<Guid> questionIds);
    }
}
