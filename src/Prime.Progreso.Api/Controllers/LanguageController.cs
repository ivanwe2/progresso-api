using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/languages")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _service;

        public LanguageController(ILanguageService service)
        {
            _service = service;
        }

        [HttpPost]
        [ActionName(nameof(GetLanguageAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<LanguageResponseDto>> CreateLanguageAsync([FromBody] LanguageRequestDto language)
        {
            var createdLanguage = await _service.CreateAsync(language);

            return CreatedAtAction(nameof(GetLanguageAsync), new { id = createdLanguage.Id }, createdLanguage);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetLanguageAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<LanguageResponseDto>> GetLanguageAsync([FromRoute] Guid id)
        {
            var language = await _service.GetByIdAsync(id);

            return Ok(language);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<LanguageResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var languages = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(languages);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditLanguageAsync([FromRoute] Guid id, [FromBody] LanguageRequestDto language)
        {
            await _service.UpdateAsync(id, language);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteLanguageAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
