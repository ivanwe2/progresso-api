using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/question-categories")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class QuestionCategoryController : ControllerBase
    {
        private readonly IQuestionCategoryService _questionCategoryService;

        public QuestionCategoryController(IQuestionCategoryService questionCategoryService)
        {
            _questionCategoryService = questionCategoryService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuestionCategoryResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var questionCategory = await _questionCategoryService.GetByIdAsync(id);

            return Ok(questionCategory);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<QuestionCategoryResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var categories = await _questionCategoryService.GetPageAsync(pagingInfo.Page, pagingInfo.Size);

            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<QuestionCategoryResponseDto>> CreateAsync([FromBody] QuestionCategoryRequestDto dto)
        {
            var createdCategory = await _questionCategoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _questionCategoryService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] QuestionCategoryRequestDto dto)
        {
            await _questionCategoryService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}
