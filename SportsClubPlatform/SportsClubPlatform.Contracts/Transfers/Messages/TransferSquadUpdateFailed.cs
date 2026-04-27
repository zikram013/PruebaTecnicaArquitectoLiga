using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when squad update fails.
    /// </summary>
    public sealed record TransferSquadUpdateFailed(
        int TransferId,
        string Reason);
}
