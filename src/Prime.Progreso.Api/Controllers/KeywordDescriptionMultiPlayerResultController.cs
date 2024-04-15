using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/keyword-descriptions/results/multi-player")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class KeywordDescriptionMultiPlayerResultController : Controller
    {
        private readonly IKeywordDescriptionMultiPlayerResultService _service;

        public KeywordDescriptionMultiPlayerResultController(IKeywordDescriptionMultiPlayerResultService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<KeywordDescriptionMultiPlayerResultResponseDto>> CreateKeywordDescriptionMultiPlayerResultAsync(
            [FromBody] KeywordDescriptionMultiPlayerResultRequestDto dto)
        {
            var createdKeywordDescriptionMultiPlayerResult = await _service.CreateAsync(dto);

            return Created(
                new Uri(Request.GetEncodedUrl() + "/" + createdKeywordDescriptionMultiPlayerResult.Id),
                createdKeywordDescriptionMultiPlayerResult);
        }
    }
}
