using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Continues the orchestration after payment processing.
    /// </summary>
    public sealed class TransferPaymentProcessedConsumer : IConsumer<TransferPaymentProcessed>
    {
        private readonly AppDbContext _dbContext;

        public TransferPaymentProcessedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransferPaymentProcessed> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkSquadUpdateRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new UpdateTransferSquads(
                    TransferId: transfer.Id,
                    PlayerId: transfer.PlayerId,
                    SourceClubId: transfer.SourceClubId,
                    DestinationClubId: transfer.DestinationClubId),
                context.CancellationToken);
        }
    }
}
