using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/answer-choices")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class AnswerChoiceController : Controller
    {
        private readonly IAnswerChoiceService _service;

        public AnswerChoiceController(IAnswerChoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<AnswerChoiceResponseDto>> CreateAnswerChoiceAsync(
            [FromBody] AnswerChoiceRequestDto answerChoice)
        {
            var createdAnswerChoice = await _service.CreateAsync(answerChoice);

            return CreatedAtAction(nameof(GetAnswerChoiceAsync), new { id = createdAnswerChoice.Id }, createdAnswerChoice);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAnswerChoiceAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<AnswerChoiceResponseDto>> GetAnswerChoiceAsync([FromRoute] Guid id)
        {
            var answerChoice = await _service.GetByIdAsync(id);

            return Ok(answerChoice);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<AnswerChoiceResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var answerChoices = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(answerChoices);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> EditAnswerChoiceAsync([FromRoute] Guid id,
                                                             [FromBody] AnswerChoiceRequestDto answerChoice)
        {
            await _service.UpdateAsync(id, answerChoice);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAnswerChoiceAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
