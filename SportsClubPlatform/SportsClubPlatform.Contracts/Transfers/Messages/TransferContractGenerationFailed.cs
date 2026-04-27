using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when contract generation fails.
    /// </summary>
    public sealed record TransferContractGenerationFailed(
        int TransferId,
        string Reason);
}
