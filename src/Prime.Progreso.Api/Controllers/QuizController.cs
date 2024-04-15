using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Pagination.Quiz;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/quizzes")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<QuizResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var responseDto = await _quizService.GetByIdAsync(id);

            return Ok(responseDto);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<QuizResponseDto>>> GetPageAsync([FromQuery] QuizesPagingInfo pagingInfo)
        {
            var response = await _quizService.GetPageAsync(pagingInfo);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuizResponseDto>> CreateAsync([FromBody] QuizRequestDto requestDto)
        {
            var responseDto = await _quizService.CreateAsync(requestDto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = responseDto.Id }, responseDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _quizService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] QuizRequestDto dto)
        {
            await _quizService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpGet("statistics")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<ActionResult<QuizzesStatisticsPaginatedDto>> GetStatisticsAsync(
            [FromQuery] QuizStatisticsPagingInfo pagingInfo)
        {
            var response = await _quizService.GetPagedStatisticsAsync(pagingInfo);

            return Ok(response);
        }

        [HttpGet("{id}/statistics")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<ActionResult<QuizStatisticsResponseDto>> GetStatisticsByIdAsync([FromRoute] Guid id)
        {
            var responseDto = await _quizService.GetStatisticsByIdAsync(id);

            return Ok(responseDto);
        }
    }
}
