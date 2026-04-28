using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Validates whether the destination club has enough budget.
    /// </summary>
    public sealed class ValidateTransferBudgetConsumer : IConsumer<ValidateTransferBudget>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public ValidateTransferBudgetConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
        }

        public async Task Consume(ConsumeContext<ValidateTransferBudget> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            var budget = await _dbContext.Budgets
                .SingleOrDefaultAsync(
                    x => x.ClubId == message.DestinationClubId,
                    context.CancellationToken);

            if (budget is null)
            {
                const string reason = "Destination club budget was not found.";

                transfer.MarkAsFailed(reason);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await _auditService.AddAsync(
                    message.TransferId,
                    "Budget Validation",
                    "Failed",
                    reason,
                    context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidationFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            try
            {
                budget.Reserve(message.OfferAmount);
                transfer.MarkBudgetValidated();

                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await _auditService.AddAsync(
                    message.TransferId,
                    "Budget Validation",
                    "Success",
                    $"Budget reserved for amount {message.OfferAmount}.",
                    context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidated(message.TransferId),
                    context.CancellationToken);
            }
            catch (Exception exception)
            {
                transfer.MarkAsFailed(exception.Message);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await _auditService.AddAsync(
                    message.TransferId,
                    "Budget Validation",
                    "Failed",
                    exception.Message,
                    context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidationFailed(message.TransferId, exception.Message),
                    context.CancellationToken);
            }
        }
    }
}
