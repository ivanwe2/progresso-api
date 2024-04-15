using Microsoft.AspNetCore.Mvc;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("API is up and running.");
        }
    }
}
