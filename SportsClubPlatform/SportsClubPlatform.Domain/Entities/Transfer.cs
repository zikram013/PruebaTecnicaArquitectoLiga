using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;
using SportsClubPlatform.Domain.Enums;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a player transfer process between two clubs.
    /// </summary>
    public sealed class Transfer : BaseEntity
    {
        private Transfer()
        {
        }

        private Transfer(
            int playerId,
            int sourceClubId,
            int destinationClubId,
            decimal offerAmount,
            decimal salaryProposed)
        {
            if (playerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playerId));
            }

            if (sourceClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceClubId));
            }

            if (destinationClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationClubId));
            }

            if (sourceClubId == destinationClubId)
            {
                throw new InvalidOperationException("Source and destination clubs must be different.");
            }

            if (offerAmount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offerAmount));
            }

            if (salaryProposed <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(salaryProposed));
            }

            PlayerId = playerId;
            SourceClubId = sourceClubId;
            DestinationClubId = destinationClubId;
            OfferAmount = offerAmount;
            SalaryProposed = salaryProposed;
            Status = TransferStatus.OfferSubmitted;
            CreatedAtUtc = DateTime.UtcNow;
            LastUpdatedUtc = DateTime.UtcNow;
        }

        public int PlayerId { get; private set; }

        public int SourceClubId { get; private set; }

        public int DestinationClubId { get; private set; }

        public decimal OfferAmount { get; private set; }

        public decimal SalaryProposed { get; private set; }

        public TransferStatus Status { get; private set; }

        public string? ErrorMessage { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime LastUpdatedUtc { get; private set; }

        public static Transfer Create(
            int playerId,
            int sourceClubId,
            int destinationClubId,
            decimal offerAmount,
            decimal salaryProposed)
        {
            return new Transfer(
                playerId,
                sourceClubId,
                destinationClubId,
                offerAmount,
                salaryProposed);
        }

        public void MarkStatus(TransferStatus status)
        {
            Status = status;
            LastUpdatedUtc = DateTime.UtcNow;
        }

        public void MarkAsFailed(string errorMessage)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

            Status = TransferStatus.Failed;
            ErrorMessage = errorMessage;
            LastUpdatedUtc = DateTime.UtcNow;
        }

        public void MarkAsCompensated()
        {
            Status = TransferStatus.Compensated;
            LastUpdatedUtc = DateTime.UtcNow;
        }
    }
}
