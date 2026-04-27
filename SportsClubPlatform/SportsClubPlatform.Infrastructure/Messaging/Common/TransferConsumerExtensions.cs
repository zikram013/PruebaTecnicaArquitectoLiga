using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Infrastructure.Persistence;

namespace SportsClubPlatform.Infrastructure.Messaging.Common
{
    /// <summary>
    /// Shared helper methods for transfer consumers.
    /// </summary>
    internal static class TransferConsumerExtensions
    {
        public static async Task<Transfer> GetTransferOrThrowAsync(
            this AppDbContext dbContext,
            int transferId,
            CancellationToken cancellationToken)
        {
            return await dbContext.Transfers
                .SingleOrDefaultAsync(x => x.Id == transferId, cancellationToken)
                ?? throw new InvalidOperationException($"Transfer '{transferId}' was not found.");
        }
    }
}
