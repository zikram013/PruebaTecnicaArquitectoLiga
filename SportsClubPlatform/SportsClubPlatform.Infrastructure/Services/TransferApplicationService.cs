using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Transfers;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Services
{
    /// <summary>
    /// Transfer application service backed by EF Core persistence.
    /// </summary>
    public sealed class TransferApplicationService : ITransferApplicationService
    {
        private readonly AppDbContext _dbContext;

        public TransferApplicationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransferResponse> SubmitTransferOfferAsync(
            SubmitTransferOfferRequest request,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);

            Club? destinationClub = await _dbContext.Clubs
                .Include(x => x.Budget)
                .SingleOrDefaultAsync(x => x.Id == request.DestinationClubId, cancellationToken);

            if (destinationClub is null)
            {
                throw new InvalidOperationException("Destination club was not found.");
            }

            Player? player = await _dbContext.Players
                .SingleOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

            if (player is null)
            {
                throw new InvalidOperationException("Player was not found.");
            }

            PlayerContract? activeContract = await _dbContext.PlayerContracts
                .SingleOrDefaultAsync(
                    x => x.PlayerId == request.PlayerId && x.IsActive,
                    cancellationToken);

            if (activeContract is null || !activeContract.IsValidOn(DateTime.UtcNow))
            {
                throw new InvalidOperationException("The player does not have a valid active contract.");
            }

            int sourceClubId = activeContract.ClubId;

            if (sourceClubId == request.DestinationClubId)
            {
                throw new InvalidOperationException("Source and destination clubs must be different.");
            }

            if (destinationClub.Budget is null)
            {
                throw new InvalidOperationException("Destination club does not have a configured budget.");
            }

            destinationClub.Budget.Reserve(request.OfferAmount);

            Transfer transfer = Transfer.Create(
                playerId: request.PlayerId,
                sourceClubId: sourceClubId,
                destinationClubId: request.DestinationClubId,
                offerAmount: request.OfferAmount,
                salaryProposed: request.SalaryProposed);

            _dbContext.Transfers.Add(transfer);
            await _dbContext.SaveChangesAsync(cancellationToken);

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
