using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Players
{
    /// <summary>
    /// API response representing a player.
    /// </summary>
    public sealed class PlayerResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public decimal MarketValue { get; set; }

        public int CurrentClubId { get; set; }

        public string? CurrentClubName { get; set; }

        public bool HasActiveContract { get; set; }
    }
}
