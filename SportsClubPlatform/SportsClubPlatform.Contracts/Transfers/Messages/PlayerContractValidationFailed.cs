using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when player contract validation fails.
    /// </summary>
    public sealed record PlayerContractValidationFailed(
        int TransferId,
        string Reason);
}
