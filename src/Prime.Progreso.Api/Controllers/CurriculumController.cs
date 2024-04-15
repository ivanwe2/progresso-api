using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;
using Prime.Progreso.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Prime.Progreso.Domain.Pagination.Curriculum;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/curriculums")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _service;

        public CurriculumController(ICurriculumService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<ActionResult<CurriculumResponseDto>> CreateCurriculumAsync(
            [FromBody] CurriculumRequestDto curriculum)
        {
            var createdCurriculum = await _service.CreateAsync(curriculum);

            return CreatedAtAction(nameof(GetCurriculumAsync), new { id = createdCurriculum.Id }, createdCurriculum);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetCurriculumAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<CurriculumResponseDto>> GetCurriculumAsync([FromRoute] Guid id)
        {
            var curriculum = await _service.GetByIdAsync(id);

            return Ok(curriculum);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<CurriculumResponseDto>>> GetPageAsync(
            [FromQuery] CurriculumsPagingInfo pagingInfo)
        {
            var curriculums = await _service.GetPageAsync(pagingInfo);

            return Ok(curriculums);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditCurriculumAsync([FromRoute] Guid id,
            [FromBody] CurriculumRequestDto curriculum)
        {
            await _service.UpdateAsync(id, curriculum);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteCurriculumAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
