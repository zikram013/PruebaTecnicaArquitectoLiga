using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers
{
    /// <summary>
    /// API response representing a transfer process.
    /// </summary>
    public sealed class TransferResponse
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int SourceClubId { get; set; }
        public int DestinationClubId { get; set; }
        public decimal OfferAmount { get; set; }
        public decimal SalaryProposed { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}
