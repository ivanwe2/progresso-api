using Microsoft.Extensions.Options;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.PathDtos;

namespace Prime.Progreso.Data.Repositories
{
    public class BpmnDiagramFileRepository : FileRepository<PathToBpmnDirectory>, IBpmnDiagramFileRepository
    {
        public BpmnDiagramFileRepository(IOptions<PathToBpmnDirectory> pathOptions)
            : base(pathOptions)
        {
        }
    }
}
