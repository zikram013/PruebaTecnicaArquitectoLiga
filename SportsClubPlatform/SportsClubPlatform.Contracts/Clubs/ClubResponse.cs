using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Clubs
{
    /// <summary>
    /// API response representing a club.
    /// </summary>
    public sealed class ClubResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public decimal? AvailableBudget { get; set; }

        public string? Currency { get; set; }
    }
}
