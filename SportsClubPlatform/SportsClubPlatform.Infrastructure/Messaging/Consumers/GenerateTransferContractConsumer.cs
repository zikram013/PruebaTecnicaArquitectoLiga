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
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Messaging.Consumers
{
    /// <summary>
    /// Generates the new player contract with the destination club.
    /// </summary>
    public sealed class GenerateTransferContractConsumer : IConsumer<GenerateTransferContract>
    {
        private readonly AppDbContext _dbContext;
        private readonly ITransferAuditService _auditService;

        public GenerateTransferContractConsumer(
            AppDbContext dbContext,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _auditService = auditService;
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

                await _auditService.AddAsync(
                    message.TransferId,
                    "Contract Generation",
                    "Failed",
                    reason,
                    context.CancellationToken);

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

            GeneratedContract generatedContract = new(
                transferId: message.TransferId,
                playerId: message.PlayerId,
                destinationClubId: message.DestinationClubId,
                annualSalary: message.SalaryProposed,
                documentReference: $"TR-{message.TransferId}-{DateTime.UtcNow:yyyyMMddHHmmss}");

            _dbContext.PlayerContracts.Add(newContract);
            _dbContext.GeneratedContracts.Add(generatedContract);

            transfer.MarkContractGenerated();

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            await _auditService.AddAsync(
                message.TransferId,
                "Contract Generation",
                "Success",
                $"Generated contract reference {generatedContract.DocumentReference}.",
                context.CancellationToken);

            await context.Publish(
                new TransferContractGenerated(message.TransferId),
                context.CancellationToken);
        }
    }
}
