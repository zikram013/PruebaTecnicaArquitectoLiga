using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting transfer payment processing.
    /// </summary>
    public sealed record ProcessTransferPayment(
        int TransferId,
        int SourceClubId,
        int DestinationClubId,
        decimal Amount);
}
