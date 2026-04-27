using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Validates whether the destination club has enough budget.
    /// </summary>
    public sealed class ValidateTransferBudgetConsumer : IConsumer<ValidateTransferBudget>
    {
        private readonly AppDbContext _dbContext;

        public ValidateTransferBudgetConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
                transfer.MarkAsFailed("Destination club budget was not found.");
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidationFailed(
                        message.TransferId,
                        "Destination club budget was not found."),
                    context.CancellationToken);

                return;
            }

            try
            {
                budget.Reserve(message.OfferAmount);
                transfer.MarkBudgetValidated();

                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidated(message.TransferId),
                    context.CancellationToken);
            }
            catch (Exception exception)
            {
                transfer.MarkAsFailed(exception.Message);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new TransferBudgetValidationFailed(
                        message.TransferId,
                        exception.Message),
                    context.CancellationToken);
            }
        }
    }
}
