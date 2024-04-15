using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Api.Constants.FileContentTypes;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestModels.BpmnDiagram;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/bpmn-diagrams")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
    public class BpmnDiagramController : ControllerBase
    {
        private readonly IBpmnDiagramService _service;

        public BpmnDiagramController(IBpmnDiagramService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<FileStreamResult>> GetBpmnDiagramByIdAsync([FromRoute] Guid id)
        {
            var fileStream = await _service.GetFileByIdAsync(id);

            if (fileStream is null)
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream.BaseStream, FileTypeConstants.Xml);
        }

        [HttpGet]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<BpmnDiagramGetMetadataResponseDto>>> GetMetadataForAllDiagramsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var result = await _service.GetMetadataPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<BpmnDiagramGetMetadataResponseDto>> CreateBpmnDiagramAsync([FromForm] BpmnDiagramCreateRequestForm request)
        {
            var created = await _service.CreateAsync(request);

            return Ok(created);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteBpmnDiagramAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateBpmnDiagramAsync([FromRoute] Guid id, [FromForm] BpmnDiagramUpdateRequestForm request)
        {
            await _service.UpdateAsync(id, request);

            return NoContent();
        }
    }
}
