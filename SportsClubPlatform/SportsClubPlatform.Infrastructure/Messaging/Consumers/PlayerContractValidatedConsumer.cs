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
    /// Continues the orchestration after player contract validation.
    /// </summary>
    public sealed class PlayerContractValidatedConsumer : IConsumer<PlayerContractValidated>
    {
        private readonly AppDbContext _dbContext;

        public PlayerContractValidatedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<PlayerContractValidated> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkPaymentRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new ProcessTransferPayment(
                    TransferId: transfer.Id,
                    SourceClubId: transfer.SourceClubId,
                    DestinationClubId: transfer.DestinationClubId,
                    Amount: transfer.OfferAmount),
                context.CancellationToken);
        }
    }
}
