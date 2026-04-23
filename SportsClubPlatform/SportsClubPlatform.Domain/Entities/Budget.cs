using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents the available transfer budget of a club.
    /// </summary>
    public sealed class Budget : BaseEntity
    {
        private Budget()
        {
        }

        public Budget(int clubId, decimal availableAmount, string currency)
        {
            if (clubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clubId));
            }

            if (availableAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(availableAmount));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(currency);

            ClubId = clubId;
            AvailableAmount = availableAmount;
            Currency = currency;
            LastUpdatedUtc = DateTime.UtcNow;
        }

        public int ClubId { get; private set; }

        public Club? Club { get; private set; }

        public decimal AvailableAmount { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public DateTime LastUpdatedUtc { get; private set; }

        public void Reserve(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (AvailableAmount < amount)
            {
                throw new InvalidOperationException("The club does not have enough available budget.");
            }

            AvailableAmount -= amount;
            LastUpdatedUtc = DateTime.UtcNow;
        }

        public void Release(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            AvailableAmount += amount;
            LastUpdatedUtc = DateTime.UtcNow;
        }
    }
}
