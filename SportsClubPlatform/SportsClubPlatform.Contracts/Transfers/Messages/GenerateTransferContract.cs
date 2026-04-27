using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Messages
{
    /// <summary>
    /// Command requesting generation of the new player contract.
    /// </summary>
    public sealed record GenerateTransferContract(
        int TransferId,
        int PlayerId,
        int DestinationClubId,
        decimal SalaryProposed);
}
