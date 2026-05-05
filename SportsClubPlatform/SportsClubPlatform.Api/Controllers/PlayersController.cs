using Microsoft.AspNetCore.Mvc;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Players;

namespace SportsClubPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class PlayersController : ControllerBase
    {
        private readonly ICatalogApplicationService _catalogApplicationService;

        public PlayersController(ICatalogApplicationService catalogApplicationService)
        {
            _catalogApplicationService = catalogApplicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PlayerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlayers(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<PlayerResponse> response =
                await _catalogApplicationService.GetPlayersAsync(cancellationToken);

            return Ok(response);
        }
    }
}
