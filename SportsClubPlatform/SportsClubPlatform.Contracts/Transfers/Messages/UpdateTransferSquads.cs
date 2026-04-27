using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting squad update after payment.
    /// </summary>
    public sealed record UpdateTransferSquads(
        int TransferId,
        int PlayerId,
        int SourceClubId,
        int DestinationClubId);
}
