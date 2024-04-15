using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.Projects;
using Prime.Progreso.Domain.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<ProjectResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);

            return Ok(project);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetPageAsync([FromQuery] PagingInfo paginator)
        {
            var projects = await _projectService.GetPageAsync(paginator.Page, paginator.Size);

            return Ok(projects);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<ProjectResponseDto>> CreateAsync([FromBody] ProjectRequestDto dto)
        {
            var createdProject = await _projectService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] ProjectRequestDto dto)
        {
            await _projectService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminRole)]
        [SwaggerOperation(Description = "ADMIN role required")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _projectService.DeleteAsync(id);

            return NoContent();
        }
    }
}
