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

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Applies compensating actions when a transfer fails.
    /// </summary>
    public sealed class CompensateTransferConsumer : IConsumer<CompensateTransfer>
    {
        private readonly AppDbContext _dbContext;

        public CompensateTransferConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CompensateTransfer> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            transfer.MarkCompensationRequested(message.Reason);

            if (ShouldReleaseBudget(transfer.Status))
            {
                var budget = await _dbContext.Budgets
                    .SingleOrDefaultAsync(
                        x => x.ClubId == transfer.DestinationClubId,
                        context.CancellationToken);

                budget?.Release(transfer.OfferAmount);
            }

            var player = await _dbContext.Players
                .SingleOrDefaultAsync(
                    x => x.Id == transfer.PlayerId,
                    context.CancellationToken);

            if (player is not null && player.CurrentClubId == transfer.DestinationClubId)
            {
                player.ChangeClub(transfer.SourceClubId);
            }

            transfer.MarkAsCompensated();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new TransferCompensated(message.TransferId),
                context.CancellationToken);
        }

        private static bool ShouldReleaseBudget(TransferStatus status)
        {
            return status is
                TransferStatus.BudgetValidated or
                TransferStatus.PlayerContractValidationRequested or
                TransferStatus.PlayerContractValidated or
                TransferStatus.PaymentRequested or
                TransferStatus.PaymentProcessed or
                TransferStatus.SquadUpdateRequested or
                TransferStatus.SquadsUpdated or
                TransferStatus.ContractGenerationRequested or
                TransferStatus.ContractGenerated or
                TransferStatus.NotificationRequested;
        }
    }
}
