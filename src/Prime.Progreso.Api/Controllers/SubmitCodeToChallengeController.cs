using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.SolutionDtos;
using Prime.Progreso.Domain.RequestForms.Solution;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/coding-challenges/solutions")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class SubmitCodeToChallengeController : Controller
    {
        private readonly ISolutionService _service;

        public SubmitCodeToChallengeController(ISolutionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminInternRoles)]
        [SwaggerOperation(Description = "ADMIN or INTERN role required")]
        public async Task<IActionResult> SubmitCodeAsync([FromBody] SolutionRequestDto dto)
        {
            var solution = await _service.SubmitCodeAsync(dto);

            return CreatedAtAction(nameof(GetCodeByInternAndChallengeIdsAsync),
                new { id = solution.AssignmentId }, solution);
        }

        [HttpGet]
        [ActionName(nameof(GetCodeByInternAndChallengeIdsAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<SolutionResponseDto>> GetCodeByInternAndChallengeIdsAsync(
            [FromQuery] SolutionCodeGetRequest solutionRequest)
        {
            var solution = await _service.GetCodeByCodingChallengeIdAsync(solutionRequest.InternId, 
                solutionRequest.CodingChallengeId);

            return Ok(solution);
        }
    }
}
