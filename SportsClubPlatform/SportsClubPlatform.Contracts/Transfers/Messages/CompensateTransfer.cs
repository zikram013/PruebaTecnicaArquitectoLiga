using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting transfer compensation.
    /// </summary>
    public sealed record CompensateTransfer(
        int TransferId,
        string Reason);
}
