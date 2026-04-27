using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when budget validation fails.
    /// </summary>
    public sealed record TransferBudgetValidationFailed(
        int TransferId,
        string Reason);
}
