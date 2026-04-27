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
    /// Continues the orchestration after squad update.
    /// </summary>
    public sealed class TransferSquadsUpdatedConsumer : IConsumer<TransferSquadsUpdated>
    {
        private readonly AppDbContext _dbContext;

        public TransferSquadsUpdatedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransferSquadsUpdated> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkContractGenerationRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new GenerateTransferContract(
                    TransferId: transfer.Id,
                    PlayerId: transfer.PlayerId,
                    DestinationClubId: transfer.DestinationClubId,
                    SalaryProposed: transfer.SalaryProposed),
                context.CancellationToken);
        }
    }
}
