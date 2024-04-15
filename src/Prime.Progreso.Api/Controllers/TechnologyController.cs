using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Pagination.Technology;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/technologies")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class TechnologyController : Controller
    {
        private readonly ITechnologyService _service;

        public TechnologyController(ITechnologyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<TechnologyResponseDto>> CreateTechnologyAsync([FromBody] TechnologyRequestDto technology)
        {
            var createdTechnology = await _service.CreateAsync(technology);

            return CreatedAtAction(nameof(GetTechnologyAsync), new { id = createdTechnology.Id }, createdTechnology);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetTechnologyAsync))]
        public async Task<ActionResult<TechnologyResponseDto>> GetTechnologyAsync([FromRoute] Guid id)
        {
            var technology = await _service.GetByIdAsync(id);

            return Ok(technology);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyResponseDto>>> GetPageAsync(
            [FromQuery] TechnologiesPagingInfo pagingInfo)
        {
            var technologies = await _service.GetPageAsync(pagingInfo);

            return Ok(technologies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTechnologyAsync([FromRoute] Guid id, [FromBody] TechnologyRequestDto technology)
        {
            await _service.UpdateAsync(id, technology);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnologyAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
