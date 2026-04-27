using MassTransit;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Starts the transfer orchestration when an offer is submitted.
    /// </summary>
    public sealed class TransferOfferSubmittedConsumer : IConsumer<TransferOfferSubmitted>
    {
        private readonly AppDbContext _dbContext;

        public TransferOfferSubmittedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransferOfferSubmitted> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            transfer.MarkBudgetValidationRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new ValidateTransferBudget(
                    TransferId: message.TransferId,
                    DestinationClubId: message.DestinationClubId,
                    OfferAmount: message.OfferAmount),
                context.CancellationToken);
        }
    }
}
