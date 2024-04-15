using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/keywords/results/multi-player")]
    [ApiController]
    public class KeywordMultiPlayerResultController : Controller
    {
        private readonly IKeywordMultiPlayerResultService _service;

        public KeywordMultiPlayerResultController(IKeywordMultiPlayerResultService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<KeywordMultiPlayerResultResponseDto>> CreateKeywordMultiPlayerResultAsync(
            [FromBody] KeywordMultiPlayerResultRequestDto keywordMultiPlayerResult)
        {
            var createdKeywordMultiPlayerResult = await _service.CreateAsync(keywordMultiPlayerResult);

            return Created(new Uri(Request.GetEncodedUrl() + "/" + createdKeywordMultiPlayerResult.Id), 
                createdKeywordMultiPlayerResult);
        }
    }
}
