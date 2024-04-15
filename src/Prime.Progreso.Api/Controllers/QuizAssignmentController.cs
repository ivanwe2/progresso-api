using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/quiz-assignments")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class QuizAssignmentController : ControllerBase
    {
        private readonly IQuizAssignmentService _service;

        public QuizAssignmentController(IQuizAssignmentService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuizAssignmentResponseDto>> CreateQuizAssignmentAsync(
            [FromBody] QuizAssignmentRequestDto quizAssignment)
        {
            var createdQuizAssignment = await _service.CreateAsync(quizAssignment);

            return CreatedAtAction(nameof(GetQuizAssignmentAsync), new { id = createdQuizAssignment.Id }, createdQuizAssignment);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetQuizAssignmentAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<QuizAssignmentResponseDto>> GetQuizAssignmentAsync([FromRoute] Guid id)
        { 
            var quizAssignment = await _service.GetByIdAsync(id);

            return Ok(quizAssignment);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<QuizAssignmentResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var quizAssignments = await _service.GetPageAsync(pagingInfo);

            return Ok(quizAssignments);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditQuizAssignmentAsync([FromRoute] Guid id,
                                                             [FromBody] QuizAssignmentRequestDto quizAssignment)
        {
            await _service.UpdateAsync(id, quizAssignment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteQuizAssignmentAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
