using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Simulates transfer payment processing.
    /// </summary>
    public sealed class ProcessTransferPaymentConsumer : IConsumer<ProcessTransferPayment>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public ProcessTransferPaymentConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
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

                await _auditService.AddAsync(
                    message.TransferId,
                    "Payment Processing",
                    "Failed",
                    reason,
                    context.CancellationToken);

                await context.Publish(
                    new TransferPaymentFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            Payment payment = new(
                transferId: message.TransferId,
                sourceClubId: message.SourceClubId,
                destinationClubId: message.DestinationClubId,
                amount: message.Amount,
                currency: "EUR");

            _dbContext.Payments.Add(payment);

            transfer.MarkPaymentProcessed();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                message.TransferId,
                "Payment Processing",
                "Success",
                $"Payment processed for amount {message.Amount}.",
                context.CancellationToken);

            await context.Publish(
                new TransferPaymentProcessed(message.TransferId),
                context.CancellationToken);
        }
    }
}
