using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting notification to all involved transfer parties.
    /// </summary>
    public sealed record NotifyTransferParties(
        int TransferId,
        int PlayerId,
        int SourceClubId,
        int DestinationClubId);
}
