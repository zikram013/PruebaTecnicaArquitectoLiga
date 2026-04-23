using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a player's active employment contract with a club.
    /// </summary>
    public sealed class PlayerContract : BaseEntity
    {
        private PlayerContract()
        {
        }

        public PlayerContract(
            int playerId,
            int clubId,
            DateTime startDateUtc,
            DateTime endDateUtc,
            decimal annualSalary,
            bool isActive = true)
        {
            if (playerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playerId));
            }

            if (clubId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clubId));
            }

            if (endDateUtc < startDateUtc)
            {
                throw new ArgumentException("End date must be greater than or equal to start date.");
            }

            if (annualSalary < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(annualSalary));
            }

            PlayerId = playerId;
            ClubId = clubId;
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
            AnnualSalary = annualSalary;
            IsActive = isActive;
        }

        public int PlayerId { get; private set; }

        public Player? Player { get; private set; }

        public int ClubId { get; private set; }

        public Club? Club { get; private set; }

        public DateTime StartDateUtc { get; private set; }

        public DateTime EndDateUtc { get; private set; }

        public decimal AnnualSalary { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsValidOn(DateTime dateUtc)
        {
            return IsActive && StartDateUtc <= dateUtc && EndDateUtc >= dateUtc;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
