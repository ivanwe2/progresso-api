using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;
using Prime.Progreso.Domain.Pagination.TestCase;
using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Api.Controllers
{
    [ApiController]
    [Route("api/test-cases")]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class TestCaseController : Controller
    {
        private readonly ITestCaseService _service;

        public TestCaseController(ITestCaseService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<ActionResult<TestCaseResponseDto>> CreateTestCaseAsync([FromBody] TestCaseRequestDto testCase)
        {
            var createdTestCase = await _service.CreateAsync(testCase);

            return CreatedAtAction(nameof(GetTestCaseAsync), new { id = createdTestCase.Id }, createdTestCase);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetTestCaseAsync))]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<TestCaseResponseDto>> GetTestCaseAsync([FromRoute] Guid id)
        {
            var request = Request;

            var testCase = await _service.GetByIdAsync(id);

            return Ok(testCase);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        [SwaggerOperation(Description = "ADMIN, MENTOR or INTERN role required")]
        public async Task<ActionResult<IEnumerable<TestCaseResponseDto>>> GetPageAsync(
            [FromQuery] TestCasesPagingInfo pagingInfo)
        {
            var testCases = await _service.GetPageAsync(pagingInfo);

            return Ok(testCases);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> EditTestCaseAsync([FromRoute] Guid id, [FromBody] TestCaseRequestDto testCase)
        {
            await _service.UpdateAsync(id, testCase);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConstants.AllowAdminMentorRoles)]
        [SwaggerOperation(Description = "ADMIN or MENTOR role required")]
        public async Task<IActionResult> DeleteTestCaseAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
