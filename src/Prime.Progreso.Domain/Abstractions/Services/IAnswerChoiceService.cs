using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IAnswerChoiceService
    {
        Task<AnswerChoiceResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<AnswerChoiceResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<AnswerChoiceResponseDto> CreateAsync(AnswerChoiceRequestDto dto);
        Task UpdateAsync(Guid id, AnswerChoiceRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
