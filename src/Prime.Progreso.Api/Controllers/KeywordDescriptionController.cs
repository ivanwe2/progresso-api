using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestForms.RandomKeywordDescription;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/keyword-descriptions")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    [ApiController]
    public class KeywordDescriptionController : ControllerBase
    {
        private readonly IKeywordDescriptionService _keywordDescriptionService;

        public KeywordDescriptionController(IKeywordDescriptionService keywordDescriptionService)
        {
            _keywordDescriptionService = keywordDescriptionService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<KeywordDescriptionResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var keywordDescription = await _keywordDescriptionService.GetByIdAsync(id);

            return Ok(keywordDescription);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<KeywordDescriptionResponseDto>>> GetPageAsync([FromQuery] PagingInfo paginator)
        {
            var keywordDescriptions = await _keywordDescriptionService.GetPageAsync(paginator.Page, paginator.Size);
            return Ok(keywordDescriptions);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<KeywordDescriptionResponseDto>> CreateAsync([FromBody] KeywordDescriptionRequestDto dto)
        {
            var createdKeywordDescription = await _keywordDescriptionService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdKeywordDescription.Id }, createdKeywordDescription);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] KeywordDescriptionRequestDto dto)
        {
            await _keywordDescriptionService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _keywordDescriptionService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("random")]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<RandomKeywordDescriptionResponseDto>> GetRandomKeywordDescriptionAsync(
            [FromQuery] RandomKeywordDescriptionGetRequestForm randomKeywordDescriptionGetRequestForm)
        {
            var keywordDescription = await _keywordDescriptionService.GetRandomAsync(randomKeywordDescriptionGetRequestForm);

            if (keywordDescription is null)
                return Ok();

            return Ok(keywordDescription);
        }
    }
}
