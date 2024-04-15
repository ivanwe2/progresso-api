using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/curriculum-items")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class CurriculumItemController : Controller
    {
        private readonly ICurriculumItemService _service;

        public CurriculumItemController(ICurriculumItemService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<CurriculumItemResponseDto>> CreateCurriculumItemAsync(
            [FromBody] CurriculumItemRequestDto curriculumItem)
        {
            var createdCurriculumItem = await _service.CreateAsync(curriculumItem);

            return CreatedAtAction(nameof(GetCurriculumItemAsync), new { id = createdCurriculumItem.Id }, createdCurriculumItem);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetCurriculumItemAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<CurriculumItemResponseDto>> GetCurriculumItemAsync([FromRoute] Guid id)
        {
            var curriculumItem = await _service.GetByIdAsync(id);

            return Ok(curriculumItem);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<CurriculumItemResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var curriculumItems = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(curriculumItems);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditCurriculumItemAsync([FromRoute] Guid id, 
            [FromBody] CurriculumItemRequestDto curriculumItem)
        {
            await _service.UpdateAsync(id, curriculumItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteCurriculumItemAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
