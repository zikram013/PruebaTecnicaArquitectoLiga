using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{

    /// <summary>
    /// Represents a professional sports club.
    /// </summary>
    public sealed class Club : BaseEntity
    {
        private readonly List<Player> _players = [];

        private Club()
        {
        }

        public Club(string name, string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(country);

            Name = name;
            Country = country;
        }

        public string Name { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;
        public Budget? Budget { get; private set; }

        public IReadOnlyCollection<Player> Players => _players;

        public void AttachBudget(Budget budget)
        {
            Budget = budget ?? throw new ArgumentNullException(nameof(budget));
        }
    }
}
