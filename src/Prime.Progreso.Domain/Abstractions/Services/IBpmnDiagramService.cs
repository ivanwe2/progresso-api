using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestModels.BpmnDiagram;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IBpmnDiagramService
    {
        Task<BpmnDiagramGetMetadataResponseDto> CreateAsync(BpmnDiagramCreateRequestForm request);
        Task DeleteAsync(Guid id);
        Task<StreamReader> GetFileByIdAsync(Guid id);
        Task<PaginatedResult<BpmnDiagramGetMetadataResponseDto>> GetMetadataPageAsync(int pageIndex,int pageSize);
        Task UpdateAsync(Guid id, BpmnDiagramUpdateRequestForm request);
    }
}
