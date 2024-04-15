using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/questions")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuestionResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var responseDto = await _questionService.GetByIdAsync(id);

            return Ok(responseDto);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<QuestionResponseDto>>> GetPageAsync([FromQuery] PagingInfo pagingInfo)
        {
            var page = await _questionService.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(page);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuestionResponseDto>> CreateAsync([FromBody] QuestionRequestDto dto)
        {
            var responseDto = await _questionService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = responseDto.Id }, responseDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _questionService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] QuestionRequestDto dto)
        {
            await _questionService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}
