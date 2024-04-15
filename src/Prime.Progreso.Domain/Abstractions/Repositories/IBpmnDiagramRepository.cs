using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IBpmnDiagramRepository
    {
        Task<TOutput> CreateAsync<TOutput>(BpmnDiagramCreateRequestDto dto, string filePath);
        Task<string> DeleteAsync(Guid id);
        Task<BpmnDiagramGetFileResponseDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, BpmnDiagramUpdateRequestDto dto);
        Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber,
                                                             int pageSize,
                                                             Expression<Func<TOutput, bool>> filter = null);
        Task<PaginatedResult<BpmnDiagramGetMetadataResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber,
                                                                                               int pageSize,
                                                                                               int userId);
        Task TryValidateNewFileName(string newFileName, string fileType);
        Task<bool> IsAccessAllowedAsync(Guid id, int userId);
    }   
}
