using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when all transfer parties have been notified.
    /// </summary>
    public sealed record TransferPartiesNotified(
        int TransferId);
}
