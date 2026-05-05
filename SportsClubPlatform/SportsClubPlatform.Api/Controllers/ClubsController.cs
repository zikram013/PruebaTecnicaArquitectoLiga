using Microsoft.AspNetCore.Mvc;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Clubs;

namespace SportsClubPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class ClubsController : ControllerBase
    {
        private readonly ICatalogApplicationService _catalogApplicationService;

        public ClubsController(ICatalogApplicationService catalogApplicationService)
        {
            _catalogApplicationService = catalogApplicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ClubResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClubs(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ClubResponse> response =
                await _catalogApplicationService.GetClubsAsync(cancellationToken);

            return Ok(response);
        }
    }
}
