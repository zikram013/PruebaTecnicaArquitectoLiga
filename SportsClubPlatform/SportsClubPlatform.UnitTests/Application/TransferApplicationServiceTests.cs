using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using SportsClubPlatform.Contracts.Transfers;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.UnitTests.Application
{
    public sealed class TransferApplicationServiceTests
    {
        [Fact]
        public async Task SubmitTransferOfferAsync_Should_Create_Transfer_And_Publish_Event()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();
            await SeedValidScenarioAsync(dbContext);

            Mock<IPublishEndpoint> publishEndpointMock = new();

            ITransferAuditService auditService = new TransferAuditService(dbContext);

            TransferApplicationService service = new(
                dbContext,
                publishEndpointMock.Object,
                auditService);

            SubmitTransferOfferRequest request = new()
            {
                PlayerId = 1,
                DestinationClubId = 1,
                OfferAmount = 5_000_000m,
                SalaryProposed = 1_200_000m
            };

            // Act
            var response = await service.SubmitTransferOfferAsync(request);

            // Assert
            response.Id.Should().BeGreaterThan(0);
            response.PlayerId.Should().Be(1);
            response.SourceClubId.Should().Be(2);
            response.DestinationClubId.Should().Be(1);
            response.Status.Should().Be("OfferSubmitted");

            bool transferExists = await dbContext.Transfers.AnyAsync(x => x.Id == response.Id);
            transferExists.Should().BeTrue();

            bool auditExists = await dbContext.TransferAuditEntries
                .AnyAsync(x => x.TransferId == response.Id && x.Step == "Offer Submission");

            auditExists.Should().BeTrue();

            publishEndpointMock.Verify(
                x => x.Publish(
                    It.Is<TransferOfferSubmitted>(m => m.TransferId == response.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SubmitTransferOfferAsync_Should_Throw_When_Player_Does_Not_Exist()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();
            await SeedValidScenarioAsync(dbContext);

            Mock<IPublishEndpoint> publishEndpointMock = new();
            ITransferAuditService auditService = new TransferAuditService(dbContext);

            TransferApplicationService service = new(
                dbContext,
                publishEndpointMock.Object,
                auditService);

            SubmitTransferOfferRequest request = new()
            {
                PlayerId = 999,
                DestinationClubId = 1,
                OfferAmount = 5_000_000m,
                SalaryProposed = 1_200_000m
            };

            // Act
            Func<Task> act = async () => await service.SubmitTransferOfferAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Player was not found.");
        }

        [Fact]
        public async Task SubmitTransferOfferAsync_Should_Throw_When_Destination_Club_Does_Not_Exist()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();
            await SeedValidScenarioAsync(dbContext);

            Mock<IPublishEndpoint> publishEndpointMock = new();
            ITransferAuditService auditService = new TransferAuditService(dbContext);

            TransferApplicationService service = new(
                dbContext,
                publishEndpointMock.Object,
                auditService);

            SubmitTransferOfferRequest request = new()
            {
                PlayerId = 1,
                DestinationClubId = 999,
                OfferAmount = 5_000_000m,
                SalaryProposed = 1_200_000m
            };

            // Act
            Func<Task> act = async () => await service.SubmitTransferOfferAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Destination club was not found.");
        }

        [Fact]
        public async Task GetAuditTimelineAsync_Should_Return_Audit_Entries()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();
            await SeedValidScenarioAsync(dbContext);

            Mock<IPublishEndpoint> publishEndpointMock = new();
            ITransferAuditService auditService = new TransferAuditService(dbContext);

            TransferApplicationService service = new(
                dbContext,
                publishEndpointMock.Object,
                auditService);

            SubmitTransferOfferRequest request = new()
            {
                PlayerId = 1,
                DestinationClubId = 1,
                OfferAmount = 5_000_000m,
                SalaryProposed = 1_200_000m
            };

            TransferResponse transfer = await service.SubmitTransferOfferAsync(request);

            // Act
            var timeline = await service.GetAuditTimelineAsync(transfer.Id);

            // Assert
            timeline.Should().NotBeEmpty();
            timeline.Should().Contain(x => x.Step == "Offer Submission");
        }

        private static AppDbContext CreateDbContext()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static async Task SeedValidScenarioAsync(AppDbContext dbContext)
        {
            Club destinationClub = new("Club A", "Spain");
            Club sourceClub = new("Club B", "England");

            dbContext.Clubs.AddRange(destinationClub, sourceClub);
            await dbContext.SaveChangesAsync();

            Budget destinationBudget = new(
                clubId: destinationClub.Id,
                availableAmount: 50_000_000m,
                currency: "EUR");

            Budget sourceBudget = new(
                clubId: sourceClub.Id,
                availableAmount: 20_000_000m,
                currency: "EUR");

            dbContext.Budgets.AddRange(destinationBudget, sourceBudget);
            await dbContext.SaveChangesAsync();

            Player player = new(
                fullName: "Player X",
                dateOfBirthUtc: new DateTime(1998, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                marketValue: 7_500_000m,
                currentClubId: sourceClub.Id);

            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();

            PlayerContract contract = new(
                playerId: player.Id,
                clubId: sourceClub.Id,
                startDateUtc: new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                endDateUtc: DateTime.UtcNow.AddYears(2),
                annualSalary: 1_000_000m,
                isActive: true);

            dbContext.PlayerContracts.Add(contract);
            await dbContext.SaveChangesAsync();
        }
    }
}
