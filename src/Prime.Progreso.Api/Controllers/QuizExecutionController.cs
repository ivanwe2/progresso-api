using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Swashbuckle.AspNetCore.Annotations;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/quiz-executions")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class QuizExecutionController : Controller
    {
        private readonly IQuizExecutionService _service;

        public QuizExecutionController(IQuizExecutionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<QuizExecutionResponseDto>> CreateQuizExecutionAsync(
            [FromBody] QuizExecutionRequestDto quizExecution)
        {
            var createdQuizExecution = await _service.CreateAsync(quizExecution);

            return CreatedAtAction(nameof(GetQuizExecutionAsync), new { id = createdQuizExecution.Id }, createdQuizExecution);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetQuizExecutionAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<QuizExecutionResponseDto>> GetQuizExecutionAsync([FromRoute] Guid id)
        {
            var quizExecution = await _service.GetByIdAsync(id);

            return Ok(quizExecution);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<QuizExecutionResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var quizExecutions = await _service.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(quizExecutions);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> EditQuizExecutionAsync([FromRoute] Guid id,
                                                             [FromBody] QuizExecutionRequestDto quizExecution)
        {
            await _service.UpdateAsync(id, quizExecution);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteQuizExecutionAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
