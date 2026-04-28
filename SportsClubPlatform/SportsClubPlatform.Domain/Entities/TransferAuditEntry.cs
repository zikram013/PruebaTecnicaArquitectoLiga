using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents an immutable audit entry for a transfer process.
    /// </summary>
    public sealed class TransferAuditEntry : BaseEntity
    {
        private TransferAuditEntry()
        {
        }

        public TransferAuditEntry(
            int transferId,
            string step,
            string status,
            string message)
        {
            if (transferId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferId));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(step);
            ArgumentException.ThrowIfNullOrWhiteSpace(status);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            TransferId = transferId;
            Step = step;
            Status = status;
            Message = message;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public int TransferId { get; private set; }

        public Transfer? Transfer { get; private set; }

        public string Step { get; private set; } = string.Empty;

        public string Status { get; private set; } = string.Empty;

        public string Message { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; }
    }
}
