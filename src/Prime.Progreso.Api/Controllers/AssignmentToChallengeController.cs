using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/assignments")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class AssignmentToChallengeController : ControllerBase
    {
        private readonly IAssignmentToChallengeService _service;

        public AssignmentToChallengeController(IAssignmentToChallengeService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAssignmentAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<AssignmentResponseDto>> GetAssignmentAsync([FromRoute] Guid id)
        {
            var assignment = await _service.GetByIdAsync(id);

            return Ok(assignment);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<AssignmentResponseDto>>> GetPageAsync([FromQuery] PagingInfo pagingInfo)
        {
            var page = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(page);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<AssignmentResponseDto>> AssignInternAsync([FromBody] AssignmentRequestDto dto)
        {
            var createdAssignment = await _service.AssignInternAsync(dto);

            return CreatedAtAction(nameof(GetAssignmentAsync), new { id = createdAssignment.Id }, createdAssignment);
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UnassignInternAsync([FromBody] UnassignmentRequestDto dto)
        {
            await _service.UnassignInternAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
