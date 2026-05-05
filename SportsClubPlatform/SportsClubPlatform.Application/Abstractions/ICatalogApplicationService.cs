using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Contracts.Clubs;
using SportsClubPlatform.Contracts.Players;

namespace SportsClubPlatform.Application.Abstractions
{
    /// <summary>
    /// Application service for simple catalog queries used by the PoC demo.
    /// </summary>
    public interface ICatalogApplicationService
    {
        Task<IReadOnlyCollection<ClubResponse>> GetClubsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<PlayerResponse>> GetPlayersAsync(
            CancellationToken cancellationToken = default);
    }
}
