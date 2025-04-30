using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok(new { status = "healthy" });
        }
    }
} 