using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Event published when a transfer offer has been submitted.
    /// </summary>
    public sealed record TransferOfferSubmitted(
        int TransferId,
        int PlayerId,
        int SourceClubId,
        int DestinationClubId,
        decimal OfferAmount,
        decimal SalaryProposed);
}
