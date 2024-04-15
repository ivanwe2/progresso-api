using Prime.Progreso.Domain.Dtos.QuestionDtos;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IQuestionRepository : IBaseRepository
    {
        Task<QuestionResponseDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, QuestionRequestDto dto);
        Task<bool> DoAllQuestionsExist(List<Guid> questionIds);
        Task<List<Guid>> GetQuestionIdsByQuizIdAsync(Guid quizId);
        Task<List<QuestionStatisticsResponseDto>> CalculateSuccessRateForQuestionStatisticsAsync(
            List<QuestionStatisticsResponseDto> questions);
    }
}
