using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Domain.Entities;

namespace SportsClubPlatform.Infrastructure.Persistence.Seed
{
    /// <summary>
    /// Seeds the application database with initial data.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
        {
            if (await dbContext.Clubs.AnyAsync(cancellationToken))
            {
                return;
            }

            Club clubA = new("Club A", "Spain");
            Club clubB = new("Club B", "England");
            Club clubC = new("Club C", "Italy");

            dbContext.Clubs.AddRange(clubA, clubB, clubC);
            await dbContext.SaveChangesAsync(cancellationToken);

            Budget budgetA = new(clubA.Id, 50_000_000m, "EUR");
            Budget budgetB = new(clubB.Id, 20_000_000m, "EUR");
            Budget budgetC = new(clubC.Id, 500_000m, "EUR");

            dbContext.Budgets.AddRange(budgetA, budgetB, budgetC);
            await dbContext.SaveChangesAsync(cancellationToken);

            Player playerX = new(
                fullName: "Player X",
                dateOfBirthUtc: new DateTime(1998, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                marketValue: 7_500_000m,
                currentClubId: clubB.Id);

            dbContext.Players.Add(playerX);
            await dbContext.SaveChangesAsync(cancellationToken);

            PlayerContract contract = new(
                playerId: playerX.Id,
                clubId: clubB.Id,
                startDateUtc: new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                endDateUtc: new DateTime(2028, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                annualSalary: 1_000_000m,
                isActive: true);

            dbContext.PlayerContracts.Add(contract);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
