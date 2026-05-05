using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Clubs;
using SportsClubPlatform.Contracts.Players;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Services
{
    /// <summary>
    /// Query service for demo catalog endpoints.
    /// </summary>
    public sealed class CatalogApplicationService : ICatalogApplicationService
    {
        private readonly AppDbContext _dbContext;

        public CatalogApplicationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<ClubResponse>> GetClubsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Clubs
                .AsNoTracking()
                .Select(club => new ClubResponse
                {
                    Id = club.Id,
                    Name = club.Name,
                    Country = club.Country,
                    AvailableBudget = club.Budget == null ? null : club.Budget.AvailableAmount,
                    Currency = club.Budget == null ? null : club.Budget.Currency
                })
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<PlayerResponse>> GetPlayersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players
                .AsNoTracking()
                .Select(player => new PlayerResponse
                {
                    Id = player.Id,
                    FullName = player.FullName,
                    MarketValue = player.MarketValue,
                    CurrentClubId = player.CurrentClubId,
                    CurrentClubName = player.CurrentClub == null ? null : player.CurrentClub.Name,
                    HasActiveContract = player.Contracts.Any(contract => contract.IsActive)
                })
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
