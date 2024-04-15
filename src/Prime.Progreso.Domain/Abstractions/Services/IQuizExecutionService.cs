using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IQuizExecutionService
    {
        Task<QuizExecutionResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<QuizExecutionResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<QuizExecutionResponseDto> CreateAsync(QuizExecutionRequestDto dto);
        Task UpdateAsync(Guid id, QuizExecutionRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
