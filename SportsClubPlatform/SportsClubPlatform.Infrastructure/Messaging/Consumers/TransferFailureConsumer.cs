using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SportsClubPlatform.Contracts.Transfers.Messages;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Handles transfer failure events and requests compensation.
    /// </summary>
    public sealed class TransferFailureConsumer :
        IConsumer<TransferBudgetValidationFailed>,
        IConsumer<PlayerContractValidationFailed>,
        IConsumer<TransferPaymentFailed>,
        IConsumer<TransferSquadUpdateFailed>,
        IConsumer<TransferContractGenerationFailed>,
        IConsumer<TransferNotificationFailed>
    {
        public async Task Consume(ConsumeContext<TransferBudgetValidationFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        public async Task Consume(ConsumeContext<PlayerContractValidationFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        public async Task Consume(ConsumeContext<TransferPaymentFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        public async Task Consume(ConsumeContext<TransferSquadUpdateFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        public async Task Consume(ConsumeContext<TransferContractGenerationFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        public async Task Consume(ConsumeContext<TransferNotificationFailed> context)
        {
            await PublishCompensationAsync(context, context.Message.TransferId, context.Message.Reason);
        }

        private static async Task PublishCompensationAsync(
            ConsumeContext context,
            int transferId,
            string reason)
        {
            await context.Publish(
                new CompensateTransfer(transferId, reason),
                context.CancellationToken);
        }
    }
}
