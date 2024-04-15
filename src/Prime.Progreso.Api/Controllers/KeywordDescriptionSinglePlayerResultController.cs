using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/keyword-descriptions/results/single-player")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class KeywordDescriptionSinglePlayerResultController : Controller
    {
        private readonly IKeywordDescriptionSinglePlayerResultService _service;

        public KeywordDescriptionSinglePlayerResultController(IKeywordDescriptionSinglePlayerResultService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminInternRoles)]
        [SwaggerOperation(Description = "ADMIN or INTERN role required")]
        public async Task<ActionResult<KeywordDescriptionSinglePlayerResultResponseDto>> CreateKeywordDescriptionSinglePlayerResultAsync(
            [FromBody] KeywordDescriptionSinglePlayerResultRequestDto dto)
        {
            var createdKeywordDescriptionSinglePlayerResult = await _service.CreateAsync(dto);

            return Created(
                new Uri(Request.GetEncodedUrl() + "/" + createdKeywordDescriptionSinglePlayerResult.Id), 
                createdKeywordDescriptionSinglePlayerResult);
        }
    }
}
