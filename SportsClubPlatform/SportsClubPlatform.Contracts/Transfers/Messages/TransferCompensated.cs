using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when transfer compensation has been completed.
    /// </summary>
    public sealed record TransferCompensated(
        int TransferId);
}
