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
    /// Simulates notifying all involved transfer parties.
    /// </summary>
    public sealed class NotifyTransferPartiesConsumer : IConsumer<NotifyTransferParties>
    {
        private readonly AppDbContext _dbContext;

        public NotifyTransferPartiesConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<NotifyTransferParties> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkPartiesNotified();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new TransferPartiesNotified(context.Message.TransferId),
                context.CancellationToken);
        }
    }
}
