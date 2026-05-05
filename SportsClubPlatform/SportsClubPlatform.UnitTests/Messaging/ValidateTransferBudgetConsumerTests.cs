using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Domain.Enums;
using SportsClubPlatform.Infrastructure.Messaging.Consumers;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.UnitTests.Messaging
{
    public sealed class ValidateTransferBudgetConsumerTests
    {
        [Fact]
        public async Task Consume_Should_Validate_Budget_And_Publish_Success_Event()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();

            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            dbContext.Transfers.Add(transfer);
            dbContext.Budgets.Add(new Budget(1, 50_000_000m, "EUR"));
            await dbContext.SaveChangesAsync();

            ITransferAuditService auditService = new TransferAuditService(dbContext);

            ValidateTransferBudgetConsumer consumer = new(dbContext, auditService);

            Mock<ConsumeContext<ValidateTransferBudget>> contextMock = new();

            contextMock.SetupGet(x => x.Message)
                .Returns(new ValidateTransferBudget(transfer.Id, 1, 5_000_000m));

            contextMock.SetupGet(x => x.CancellationToken)
                .Returns(CancellationToken.None);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            Transfer updatedTransfer = await dbContext.Transfers.SingleAsync(x => x.Id == transfer.Id);
            updatedTransfer.Status.Should().Be(TransferStatus.BudgetValidated);

            Budget budget = await dbContext.Budgets.SingleAsync(x => x.ClubId == 1);
            budget.AvailableAmount.Should().Be(45_000_000m);

            bool auditExists = await dbContext.TransferAuditEntries
                .AnyAsync(x => x.TransferId == transfer.Id && x.Step == "Budget Validation");

            auditExists.Should().BeTrue();

            contextMock.Verify(
                x => x.Publish(
                    It.Is<TransferBudgetValidated>(m => m.TransferId == transfer.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Consume_Should_Fail_When_Budget_Is_Not_Enough()
        {
            // Arrange
            await using AppDbContext dbContext = CreateDbContext();

            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            dbContext.Transfers.Add(transfer);
            dbContext.Budgets.Add(new Budget(1, 500_000m, "EUR"));
            await dbContext.SaveChangesAsync();

            ITransferAuditService auditService = new TransferAuditService(dbContext);

            ValidateTransferBudgetConsumer consumer = new(dbContext, auditService);

            Mock<ConsumeContext<ValidateTransferBudget>> contextMock = new();

            contextMock.SetupGet(x => x.Message)
                .Returns(new ValidateTransferBudget(transfer.Id, 1, 5_000_000m));

            contextMock.SetupGet(x => x.CancellationToken)
                .Returns(CancellationToken.None);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            Transfer updatedTransfer = await dbContext.Transfers.SingleAsync(x => x.Id == transfer.Id);
            updatedTransfer.Status.Should().Be(TransferStatus.Failed);

            contextMock.Verify(
                x => x.Publish(
                    It.Is<TransferBudgetValidationFailed>(m => m.TransferId == transfer.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
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
