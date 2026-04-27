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
    /// Continues the orchestration after contract generation.
    /// </summary>
    public sealed class TransferContractGeneratedConsumer : IConsumer<TransferContractGenerated>
    {
        private readonly AppDbContext _dbContext;

        public TransferContractGeneratedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransferContractGenerated> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkNotificationRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new NotifyTransferParties(
                    TransferId: transfer.Id,
                    PlayerId: transfer.PlayerId,
                    SourceClubId: transfer.SourceClubId,
                    DestinationClubId: transfer.DestinationClubId),
                context.CancellationToken);
        }
    }
}
