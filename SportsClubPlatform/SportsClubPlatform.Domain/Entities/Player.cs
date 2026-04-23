using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a professional player.
    /// </summary>
    public sealed class Player : BaseEntity
    {
        private readonly List<PlayerContract> _contracts = [];

        private Player()
        {
        }

        public Player(
            string fullName,
            DateTime dateOfBirthUtc,
            decimal marketValue,
            int currentClubId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

            if (marketValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(marketValue));
            }

            if (currentClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentClubId));
            }

            FullName = fullName;
            DateOfBirthUtc = dateOfBirthUtc;
            MarketValue = marketValue;
            CurrentClubId = currentClubId;
        }

        public string FullName { get; private set; } = string.Empty;

        public DateTime DateOfBirthUtc { get; private set; }

        public decimal MarketValue { get; private set; }

        public int CurrentClubId { get; private set; }

        public Club? CurrentClub { get; private set; }

        public IReadOnlyCollection<PlayerContract> Contracts => _contracts;

        public void ChangeClub(int newClubId)
        {
            if (newClubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newClubId));
            }

            CurrentClubId = newClubId;
        }
    }
}
