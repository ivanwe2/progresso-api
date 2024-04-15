using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;
using Prime.Progreso.Domain.Pagination.CodingChallenge;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/coding-challenges")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class CodingChallengeController : ControllerBase
    {
        private readonly ICodingChallengeService _codingChallengeService;

        public CodingChallengeController(ICodingChallengeService codingChallengeService)
        {
            _codingChallengeService = codingChallengeService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<CodingChallengeResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var codingChallenge = await _codingChallengeService.GetByIdAsync(id);

            return Ok(codingChallenge);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<CodingChallengeResponseDto>>> GetPageAsync([FromQuery] CodingChallengesPagingInfo pagingInfo)
        {
            var codingChallenges = await _codingChallengeService.GetPageAsync(pagingInfo);
            return Ok(codingChallenges);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<CodingChallengeResponseDto>> CreateAsync([FromBody] CodingChallengeRequestDto dto)
        {
            var createdCodingChallenge = await _codingChallengeService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdCodingChallenge.Id }, createdCodingChallenge);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CodingChallengeRequestDto dto)
        {
            await _codingChallengeService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _codingChallengeService.DeleteAsync(id);

            return NoContent();
        }
    }
}
