using MassTransit;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Transfers;
using SportsClubPlatform.Contracts.Transfers.Audit;
using SportsClubPlatform.Contracts.Transfers.Messages;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services.Auditing;

namespace SportsClubPlatform.Infrastructure.Services
{
    /// <summary>
    /// Transfer application service backed by EF Core persistence.
    /// </summary>
    public sealed class TransferApplicationService : ITransferApplicationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITransferAuditService _auditService;

        public TransferApplicationService(
            AppDbContext dbContext,
            IPublishEndpoint publishEndpoint,
            ITransferAuditService auditService)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _auditService = auditService;
        }

        public async Task<TransferResponse> SubmitTransferOfferAsync(
            SubmitTransferOfferRequest request,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);

            Player player = await _dbContext.Players
                .SingleOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken)
                ?? throw new InvalidOperationException("Player was not found.");

            PlayerContract activeContract = await _dbContext.PlayerContracts
                .SingleOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.IsActive, cancellationToken)
                ?? throw new InvalidOperationException("The player does not have an active contract.");

            if (!activeContract.IsValidOn(DateTime.UtcNow))
            {
                throw new InvalidOperationException("The player active contract is not valid.");
            }

            int sourceClubId = activeContract.ClubId;

            if (sourceClubId == request.DestinationClubId)
            {
                throw new InvalidOperationException("Source and destination clubs must be different.");
            }

            bool destinationClubExists = await _dbContext.Clubs
                .AnyAsync(x => x.Id == request.DestinationClubId, cancellationToken);

            if (!destinationClubExists)
            {
                throw new InvalidOperationException("Destination club was not found.");
            }

            Transfer transfer = Transfer.Create(
                playerId: player.Id,
                sourceClubId: sourceClubId,
                destinationClubId: request.DestinationClubId,
                offerAmount: request.OfferAmount,
                salaryProposed: request.SalaryProposed);

            _dbContext.Transfers.Add(transfer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _auditService.AddAsync(
                transfer.Id,
                "Offer Submission",
                "Success",
                $"Transfer offer submitted for player {transfer.PlayerId}.",
                cancellationToken);

            await _publishEndpoint.Publish(
                new TransferOfferSubmitted(
                    TransferId: transfer.Id,
                    PlayerId: transfer.PlayerId,
                    SourceClubId: transfer.SourceClubId,
                    DestinationClubId: transfer.DestinationClubId,
                    OfferAmount: transfer.OfferAmount,
                    SalaryProposed: transfer.SalaryProposed),
                cancellationToken);

            return MapToResponse(transfer);
        }

        public async Task<TransferResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            Transfer? transfer = await _dbContext.Transfers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return transfer is null ? null : MapToResponse(transfer);
        }

        public async Task<IReadOnlyCollection<TransferAuditEntryResponse>> GetAuditTimelineAsync(
            int transferId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.TransferAuditEntries
                .AsNoTracking()
                .Where(x => x.TransferId == transferId)
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new TransferAuditEntryResponse
                {
                    Id = x.Id,
                    TransferId = x.TransferId,
                    Step = x.Step,
                    Status = x.Status,
                    Message = x.Message,
                    CreatedAtUtc = x.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);
        }

        private static void ValidateRequest(SubmitTransferOfferRequest request)
        {
            if (request.PlayerId <= 0)
            {
                throw new InvalidOperationException("PlayerId must be greater than zero.");
            }

            if (request.DestinationClubId <= 0)
            {
                throw new InvalidOperationException("DestinationClubId must be greater than zero.");
            }

            if (request.OfferAmount <= 0)
            {
                throw new InvalidOperationException("OfferAmount must be greater than zero.");
            }

            if (request.SalaryProposed <= 0)
            {
                throw new InvalidOperationException("SalaryProposed must be greater than zero.");
            }
        }

        private static TransferResponse MapToResponse(Transfer transfer)
        {
            return new TransferResponse
            {
                Id = transfer.Id,
                PlayerId = transfer.PlayerId,
                SourceClubId = transfer.SourceClubId,
                DestinationClubId = transfer.DestinationClubId,
                OfferAmount = transfer.OfferAmount,
                SalaryProposed = transfer.SalaryProposed,
                Status = transfer.Status.ToString(),
                ErrorMessage = transfer.ErrorMessage
            };
        }
    }
}
