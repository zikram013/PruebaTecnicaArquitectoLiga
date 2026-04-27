using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when notification fails.
    /// </summary>
    public sealed record TransferNotificationFailed(
        int TransferId,
        string Reason);
}
