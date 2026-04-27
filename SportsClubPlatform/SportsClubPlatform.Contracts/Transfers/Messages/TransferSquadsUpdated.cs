using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when squads have been updated.
    /// </summary>
    public sealed record TransferSquadsUpdated(
        int TransferId);
}
