using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.Milestones;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/milestones")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class MilestoneController : ControllerBase
    {
        private readonly IMilestoneService _milestoneService;

        public MilestoneController(IMilestoneService milestoneService)
        {
            _milestoneService = milestoneService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<MilestoneResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var milestone = await _milestoneService.GetByIdAsync(id);

            return Ok(milestone);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<MilestoneResponseDto>>> GetPageAsync([FromQuery] PagingInfo paginator)
        {
            var milestones = await _milestoneService.GetPageAsync(paginator.Page, paginator.Size);
            return Ok(milestones);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<MilestoneResponseDto>> CreateAsync([FromBody] MilestoneRequestDto dto)
        {
            var createdMilestone = await _milestoneService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdMilestone.Id }, createdMilestone);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] MilestoneRequestDto dto)
        {
            await _milestoneService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _milestoneService.DeleteAsync(id);

            return NoContent();
        }
    }
}
