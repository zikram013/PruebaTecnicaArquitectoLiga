using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when the new player contract has been generated.
    /// </summary>
    public sealed record TransferContractGenerated(
        int TransferId);
}
