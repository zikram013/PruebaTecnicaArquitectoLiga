using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers
{
    /// <summary>
    /// API request for creating a new transfer offer.
    /// </summary>
    public sealed class SubmitTransferOfferRequest
    {
        public int PlayerId { get; set; }
        public int DestinationClubId { get; set; }
        public decimal OfferAmount { get; set; }
        public decimal SalaryProposed { get; set; }
    }
}
