using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting budget validation for a transfer.
    /// </summary>
    public sealed record ValidateTransferBudget(
        int TransferId,
        int DestinationClubId,
        decimal OfferAmount);
}
