using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/activities")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class ActivityController : Controller
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<ActivityResponseDto>> CreateActivityAsync([FromBody] ActivityRequestDto activity)
        {
            var createdActivity = await _service.CreateAsync(activity);

            return CreatedAtAction(nameof(GetActivityAsync), new { id = createdActivity.Id }, createdActivity);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetActivityAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<ActivityResponseDto>> GetActivityAsync([FromRoute] Guid id)
        {
            var activity = await _service.GetByIdAsync(id);

            return Ok(activity);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<ActivityResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var activities = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(activities);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditActivityAsync([FromRoute] Guid id, [FromBody] ActivityRequestDto activity)
        {   
            await _service.UpdateAsync(id, activity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteActivityAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
