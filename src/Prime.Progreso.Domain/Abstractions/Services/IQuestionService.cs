using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IQuestionService
    {
        Task<QuestionResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<QuestionResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<QuestionResponseDto> CreateAsync(QuestionRequestDto dto);
        Task UpdateAsync(Guid id, QuestionRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
