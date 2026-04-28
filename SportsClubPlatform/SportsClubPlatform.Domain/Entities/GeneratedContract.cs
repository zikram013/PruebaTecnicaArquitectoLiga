using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a generated legal contract document for a transfer.
    /// </summary>
    public sealed class GeneratedContract : BaseEntity
    {
        private GeneratedContract()
        {
        }

        public GeneratedContract(
            int transferId,
            int playerId,
            int destinationClubId,
            decimal annualSalary,
            string documentReference)
        {
            if (transferId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferId));
            }

            if (playerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playerId));
            }

            if (destinationClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationClubId));
            }

            if (annualSalary <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(annualSalary));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(documentReference);

            TransferId = transferId;
            PlayerId = playerId;
            DestinationClubId = destinationClubId;
            AnnualSalary = annualSalary;
            DocumentReference = documentReference;
            GeneratedAtUtc = DateTime.UtcNow;
            IsCancelled = false;
        }

        public int TransferId { get; private set; }

        public Transfer? Transfer { get; private set; }

        public int PlayerId { get; private set; }

        public int DestinationClubId { get; private set; }

        public decimal AnnualSalary { get; private set; }

        public string DocumentReference { get; private set; } = string.Empty;

        public DateTime GeneratedAtUtc { get; private set; }

        public bool IsCancelled { get; private set; }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
