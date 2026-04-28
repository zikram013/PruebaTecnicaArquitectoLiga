using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Completes the transfer after all parties have been notified.
    /// </summary>
    public sealed class TransferPartiesNotifiedConsumer : IConsumer<TransferPartiesNotified>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public TransferPartiesNotifiedConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
        }

        public async Task Consume(ConsumeContext<TransferPartiesNotified> context)
        {
            var transfer = await _dbContext.GetTransferOrThrowAsync(
                context.Message.TransferId,
                context.CancellationToken);

            transfer.MarkCompleted();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                context.Message.TransferId,
                "Transfer Completion",
                "Success",
                "Transfer process completed successfully.",
                context.CancellationToken);
        }
    }
}
