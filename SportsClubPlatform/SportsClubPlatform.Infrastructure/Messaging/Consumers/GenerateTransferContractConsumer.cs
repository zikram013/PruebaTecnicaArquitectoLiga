using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Messaging.Common;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Generates the new player contract with the destination club.
    /// </summary>
    public sealed class GenerateTransferContractConsumer : IConsumer<GenerateTransferContract>
    {
        private readonly AppDbContext _dbContext;

        public GenerateTransferContractConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<GenerateTransferContract> context)
        {
            var message = context.Message;

            var transfer = await _dbContext.GetTransferOrThrowAsync(
                message.TransferId,
                context.CancellationToken);

            if (message.SalaryProposed <= 0)
            {
                const string reason = "Proposed salary must be greater than zero.";

                transfer.MarkAsFailed(reason);
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await context.Publish(
                    new TransferContractGenerationFailed(message.TransferId, reason),
                    context.CancellationToken);

                return;
            }

            var activeContracts = await _dbContext.PlayerContracts
                .Where(x => x.PlayerId == message.PlayerId && x.IsActive)
                .ToListAsync(context.CancellationToken);

            foreach (var contract in activeContracts)
            {
                contract.Deactivate();
            }

            PlayerContract newContract = new(
                playerId: message.PlayerId,
                clubId: message.DestinationClubId,
                startDateUtc: DateTime.UtcNow.Date,
                endDateUtc: DateTime.UtcNow.Date.AddYears(4),
                annualSalary: message.SalaryProposed,
                isActive: true);

            _dbContext.PlayerContracts.Add(newContract);

            transfer.MarkContractGenerated();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(
                new TransferContractGenerated(message.TransferId),
                context.CancellationToken);
        }
    }
}
