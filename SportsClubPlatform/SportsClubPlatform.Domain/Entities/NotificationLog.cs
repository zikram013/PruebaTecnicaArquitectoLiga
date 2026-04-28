using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Common;

namespace SportsClubPlatform.Domain.Entities
{
    /// <summary>
    /// Represents a notification sent during a transfer process.
    /// </summary>
    public sealed class NotificationLog : BaseEntity
    {
        private NotificationLog()
        {
        }

        public NotificationLog(
            int transferId,
            string recipientType,
            string message)
        {
            if (transferId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferId));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(recipientType);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            TransferId = transferId;
            RecipientType = recipientType;
            Message = message;
            SentAtUtc = DateTime.UtcNow;
        }

        public int TransferId { get; private set; }

        public Transfer? Transfer { get; private set; }

        public string RecipientType { get; private set; } = string.Empty;

        public string Message { get; private set; } = string.Empty;

        public DateTime SentAtUtc { get; private set; }
    }
}
