using Microsoft.AspNetCore.Mvc;

namespace SportsClubPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                service = "SportsClubPlatform.Api",
                timestampUtc = DateTime.UtcNow
            });
        }
    }
}
