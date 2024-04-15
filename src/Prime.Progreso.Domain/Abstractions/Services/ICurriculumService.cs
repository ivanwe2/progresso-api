using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Curriculum;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ICurriculumService
    {
        Task<CurriculumResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<CurriculumResponseDto>> GetPageAsync(CurriculumsPagingInfo pagingInfo);
        Task<CurriculumResponseDto> CreateAsync(CurriculumRequestDto dto);
        Task UpdateAsync(Guid id, CurriculumRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
