using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting validation of the player's current contract.
    /// </summary>
    public sealed record ValidatePlayerContract(
        int TransferId,
        int PlayerId,
        int SourceClubId);
}
