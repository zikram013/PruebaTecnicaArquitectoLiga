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

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Validates the player's current active contract.
    /// </summary>
    public sealed class ValidatePlayerContractConsumer : IConsumer<ValidatePlayerContract>
    {
        private readonly AppDbContext _dbContext;

        public ValidatePlayerContractConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ValidatePlayerContract> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            var activeContract = await _dbContext.PlayerContracts
                .SingleOrDefaultAsync(
                    x =>
                        x.PlayerId == message.PlayerId &&
                        x.ClubId == message.SourceClubId &&
                        x.IsActive,
                    context.CancellationToken);

            if (activeContract is null || !activeContract.IsValidOn(DateTime.UtcNow))
            {
                const string reason = "Player contract validation failed.";

                transfer.MarkAsFailed(reason);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new PlayerContractValidationFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            transfer.MarkPlayerContractValidated();
            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new PlayerContractValidated(message.TransferId),
                context.CancellationToken);
        }
    }
}
