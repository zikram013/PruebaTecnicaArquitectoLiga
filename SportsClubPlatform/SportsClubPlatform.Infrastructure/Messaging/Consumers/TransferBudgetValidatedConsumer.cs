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
    /// Continues the orchestration after successful budget validation.
    /// </summary>
    public sealed class TransferBudgetValidatedConsumer : IConsumer<TransferBudgetValidated>
    {
        private readonly AppDbContext _dbContext;

        public TransferBudgetValidatedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransferBudgetValidated> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            transfer.MarkPlayerContractValidationRequested();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new ValidatePlayerContract(
                    TransferId: transfer.Id,
                    PlayerId: transfer.PlayerId,
                    SourceClubId: transfer.SourceClubId),
                context.CancellationToken);
        }
    }
}
