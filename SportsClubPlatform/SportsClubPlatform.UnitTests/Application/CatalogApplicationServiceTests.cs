using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services;

namespace SportsClubPlatform.UnitTests.Application
{
    public sealed class CatalogApplicationServiceTests
    {
        [Fact]
        public async Task GetClubsAsync_Should_Return_Seeded_Clubs()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();

            Club club = new("Club A", "Spain");

            dbContext.Clubs.Add(club);
            await dbContext.SaveChangesAsync();

            dbContext.Budgets.Add(new Budget(club.Id, 50_000_000m, "EUR"));
            await dbContext.SaveChangesAsync();

            CatalogApplicationService service = new(dbContext);

            // Act
            var result = await service.GetClubsAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Name.Should().Be("Club A");
            result.Single().AvailableBudget.Should().Be(50_000_000m);
        }

        [Fact]
        public async Task GetPlayersAsync_Should_Return_Seeded_Players()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();

            Club club = new("Club B", "England");

            dbContext.Clubs.Add(club);
            await dbContext.SaveChangesAsync();

            Player player = new(
                fullName: "Player X",
                dateOfBirthUtc: new DateTime(1998, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                marketValue: 7_500_000m,
                currentClubId: club.Id);

            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();

            dbContext.PlayerContracts.Add(new PlayerContract(
                playerId: player.Id,
                clubId: club.Id,
                startDateUtc: DateTime.UtcNow.AddYears(-1),
                endDateUtc: DateTime.UtcNow.AddYears(2),
                annualSalary: 1_000_000m,
                isActive: true));

            await dbContext.SaveChangesAsync();

            CatalogApplicationService service = new(dbContext);

            // Act
            var result = await service.GetPlayersAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().FullName.Should().Be("Player X");
            result.Single().HasActiveContract.Should().BeTrue();
        }

        private static AppDbContext CreateDbContext()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
