using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Contracts.Transfers.Audit
{
    /// <summary>
    /// API response representing a transfer audit entry.
    /// </summary>
    public sealed class TransferAuditEntryResponse
    {
        public int Id { get; set; }

        public int TransferId { get; set; }

        public string Step { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; }
    }
}
