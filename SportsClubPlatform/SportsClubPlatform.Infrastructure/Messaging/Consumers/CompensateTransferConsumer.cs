using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Enums;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Applies compensating actions when a transfer fails.
    /// </summary>
    public sealed class CompensateTransferConsumer : IConsumer<CompensateTransfer>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public CompensateTransferConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
        }

        public async Task Consume(ConsumeContext<CompensateTransfer> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            transfer.MarkCompensationRequested(message.Reason);

            var budget = await _dbContext.Budgets
                .SingleOrDefaultAsync(
                    x => x.ClubId == transfer.DestinationClubId,
                    context.CancellationToken);

            if (budget is not null)
            {
                budget.Release(transfer.OfferAmount);
            }

            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(
                    x => x.TransferId == transfer.Id,
                    context.CancellationToken);

            if (payment is not null)
            {
                payment.MarkAsCompensated();
            }

            var player = await _dbContext.Players
                .SingleOrDefaultAsync(
                    x => x.Id == transfer.PlayerId,
                    context.CancellationToken);

            if (player is not null && player.CurrentClubId == transfer.DestinationClubId)
            {
                player.ChangeClub(transfer.SourceClubId);
            }

            var generatedContract = await _dbContext.GeneratedContracts
                .SingleOrDefaultAsync(
                    x => x.TransferId == transfer.Id,
                    context.CancellationToken);

            if (generatedContract is not null)
            {
                generatedContract.Cancel();
            }

            transfer.MarkAsCompensated();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                transfer.Id,
                "Compensation",
                "Success",
                $"Compensation completed. Reason: {message.Reason}",
                context.CancellationToken);

            await context.Publish(
                new TransferCompensated(message.TransferId),
                context.CancellationToken);
        }
    }
}
