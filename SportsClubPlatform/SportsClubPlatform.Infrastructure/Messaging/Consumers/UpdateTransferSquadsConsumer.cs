using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Updates squads by moving the player to the destination club.
    /// </summary>
    public sealed class UpdateTransferSquadsConsumer : IConsumer<UpdateTransferSquads>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public UpdateTransferSquadsConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
        }

        public async Task Consume(ConsumeContext<UpdateTransferSquads> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            var player = await _dbContext.Players
                .SingleOrDefaultAsync(x => x.Id == message.PlayerId, context.CancellationToken);

            if (player is null)
            {
                const string reason = "Player was not found during squad update.";

                transfer.MarkAsFailed(reason);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await _auditService.AddAsync(
                    message.TransferId,
                    "Squad Update",
                    "Failed",
                    reason,
                    context.CancellationToken);

                await context.Publish(
                    new TransferSquadUpdateFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            player.ChangeClub(message.DestinationClubId);
            transfer.MarkSquadsUpdated();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                message.TransferId,
                "Squad Update",
                "Success",
                $"Player {message.PlayerId} moved to club {message.DestinationClubId}.",
                context.CancellationToken);

            await context.Publish(
                new TransferSquadsUpdated(message.TransferId),
                context.CancellationToken);
        }
    }
}
