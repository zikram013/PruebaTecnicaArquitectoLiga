using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when transfer payment fails.
    /// </summary>
    public sealed record TransferPaymentFailed(
        int TransferId,
        string Reason);
}
