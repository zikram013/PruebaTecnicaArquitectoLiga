using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when the destination club budget has been validated.
    /// </summary>
    public sealed record TransferBudgetValidated(
        int TransferId);
}
