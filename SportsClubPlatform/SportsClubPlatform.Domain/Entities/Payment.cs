using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a simulated transfer payment.
    /// </summary>
    public sealed class Payment : BaseEntity
    {
        private Payment()
        {
        }

        public Payment(
            int transferId,
            int sourceClubId,
            int destinationClubId,
            decimal amount,
            string currency)
        {
            if (transferId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferId));
            }

            if (sourceClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceClubId));
            }

            if (destinationClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationClubId));
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(currency);

            TransferId = transferId;
            SourceClubId = sourceClubId;
            DestinationClubId = destinationClubId;
            Amount = amount;
            Currency = currency;
            ProcessedAtUtc = DateTime.UtcNow;
            IsCompensated = false;
        }

        public int TransferId { get; private set; }

        public Transfer? Transfer { get; private set; }

        public int SourceClubId { get; private set; }

        public int DestinationClubId { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public DateTime ProcessedAtUtc { get; private set; }

        public bool IsCompensated { get; private set; }

        public void MarkAsCompensated()
        {
            IsCompensated = true;
        }
    }
}
