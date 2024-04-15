using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IQuizAssignmentService
    {
        Task<QuizAssignmentResponseDto> CreateAsync(QuizAssignmentRequestDto dto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Guid id, QuizAssignmentRequestDto dto);
        Task<PaginatedResult<QuizAssignmentResponseDto>> GetPageAsync(PagingInfo pagingInfo);
        Task<QuizAssignmentResponseDto> GetByIdAsync(Guid id);
    }
}
