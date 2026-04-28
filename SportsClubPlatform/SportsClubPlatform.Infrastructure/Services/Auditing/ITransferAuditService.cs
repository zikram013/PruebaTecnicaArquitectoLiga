using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Infrastructure.Services.Auditing
{
    /// <summary>
    /// Provides audit logging capabilities for transfer orchestration.
    /// </summary>
    public interface ITransferAuditService
    {
        Task AddAsync(
            int transferId,
            string step,
            string status,
            string message,
            CancellationToken cancellationToken = default);
    }
}
