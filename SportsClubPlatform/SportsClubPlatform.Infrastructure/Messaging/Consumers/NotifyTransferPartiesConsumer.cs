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
    /// Simulates notifying all involved transfer parties.
    /// </summary>
    public sealed class NotifyTransferPartiesConsumer : IConsumer<NotifyTransferParties>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public NotifyTransferPartiesConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
        }

        public async Task Consume(ConsumeContext<NotifyTransferParties> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            _dbContext.NotificationLogs.AddRange(
                new NotificationLog(
                    message.TransferId,
                    "SourceClub",
                    $"Transfer {message.TransferId} has been completed."),
                new NotificationLog(
                    message.TransferId,
                    "DestinationClub",
                    $"Transfer {message.TransferId} has been completed."),
                new NotificationLog(
                    message.TransferId,
                    "Player",
                    $"Your transfer process {message.TransferId} has been completed."));

            transfer.MarkPartiesNotified();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                message.TransferId,
                "Notification",
                "Success",
                "All transfer parties have been notified.",
                context.CancellationToken);

            await context.Publish(
                new TransferPartiesNotified(message.TransferId),
                context.CancellationToken);
        }
    }
}
