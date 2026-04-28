using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Services.Auditing
{
    /// <summary>
    /// Persists audit entries for transfer orchestration.
    /// </summary>
    public sealed class TransferAuditService : ITransferAuditService
    {
        private readonly AppDbContext _dbContext;

        public TransferAuditService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            int transferId,
            string step,
            string status,
            string message,
            CancellationToken cancellationToken = default)
        {
            TransferAuditEntry entry = new(
                transferId,
                step,
                status,
                message);

            _dbContext.TransferAuditEntries.Add(entry);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
