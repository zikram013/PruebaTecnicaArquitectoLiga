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
    /// Simulates transfer payment processing.
    /// </summary>
    public sealed class ProcessTransferPaymentConsumer : IConsumer<ProcessTransferPayment>
    {
        private readonly AppDbContext _dbContext;

        public ProcessTransferPaymentConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProcessTransferPayment> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            if (message.Amount <= 0)
            {
                const string reason = "Payment amount must be greater than zero.";

                transfer.MarkAsFailed(reason);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new TransferPaymentFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            transfer.MarkPaymentProcessed();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new TransferPaymentProcessed(message.TransferId),
                context.CancellationToken);
        }
    }
}
