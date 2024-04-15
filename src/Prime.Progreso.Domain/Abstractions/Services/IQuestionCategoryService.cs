using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IQuestionCategoryService
    {
        Task<QuestionCategoryResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<QuestionCategoryResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<QuestionCategoryResponseDto> CreateAsync(QuestionCategoryRequestDto dto);
        Task UpdateAsync(Guid id, QuestionCategoryRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
