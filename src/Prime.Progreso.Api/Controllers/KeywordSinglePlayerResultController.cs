using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/keywords/results/single-player")]
    [ApiController]
    public class KeywordSinglePlayerResultController : ControllerBase
    {
        private readonly IKeywordSinglePlayerResultService _service;

        public KeywordSinglePlayerResultController(IKeywordSinglePlayerResultService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminInternRoles)]
        [SwaggerOperation(Description = "ADMIN or INTERN role required")]
        public async Task<ActionResult<KeywordSinglePlayerResultResponseDto>> CreateKeywordSinglePlayerResultAsync(
            [FromBody] KeywordSinglePlayerResultRequestDto keywordSinglePlayerResult)
        {
            var createdKeywordSinglePlayerResult = await _service.CreateAsync(keywordSinglePlayerResult);

            return Created(new Uri(Request.GetEncodedUrl() + "/" + createdKeywordSinglePlayerResult.Id), createdKeywordSinglePlayerResult);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateIsCorrectAsync(
            [FromRoute] Guid id,
            [FromBody] KeywordSinglePlayerResultIsCorrectUpdateRequestDto isCorrectUpdateDto)
        {
            await _service.UpdateIsCorrectAsync(id, isCorrectUpdateDto);

            return NoContent();
        }
        
        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<KeywordSinglePlayerResultResponseDto>>> GetKeywordSinglePlayerResultsAsync(
            [FromQuery] KeywordSinglePlayerResultPagingInfo pagingInfo)
        {
            var keywordSinglePlayerResult = await _service.GetPageAsync(pagingInfo);

            return Ok(keywordSinglePlayerResult);
        }
    }
}
