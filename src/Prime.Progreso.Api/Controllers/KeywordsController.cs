using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Swashbuckle.AspNetCore.Annotations;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/keywords")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    [ApiController]
    public class KeywordController : ControllerBase
    {
        private readonly IKeywordService _keywordService;

        public KeywordController(IKeywordService keyDescriptionService)
        {
            _keywordService = keyDescriptionService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<KeywordResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var keyDescription = await _keywordService.GetByIdAsync(id);

            return Ok(keyDescription);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<KeywordResponseDto>>> GetPageAsync([FromQuery] PagingInfo paginator)
        {
            var keyDescriptions = await _keywordService.GetPageAsync(paginator.Page, paginator.Size);

            return Ok(keyDescriptions);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<KeywordResponseDto>> CreateAsync([FromBody] KeywordRequestDto dto)
        {
            var createdDto = await _keywordService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] KeywordRequestDto dto)
        {
            await _keywordService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _keywordService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("random")]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<RandomKeywordResponseDto>> GetRandomKeywordAsync([FromQuery] RandomKeywordRequestDto dto)
        {
            var randomKeyword = await _keywordService.GetRandomKeywordAsync(dto);

            return Ok(randomKeyword);
        }
    }
}
