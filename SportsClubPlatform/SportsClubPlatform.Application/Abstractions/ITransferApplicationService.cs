using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsClubPlatform.Contracts.Transfers;

namespace SportsClubPlatform.Application.Abstractions
{
    /// <summary>
    /// Application service for transfer use cases.
    /// </summary>
    public interface ITransferApplicationService
    {
        Task<TransferResponse> SubmitTransferOfferAsync(
            SubmitTransferOfferRequest request,
            CancellationToken cancellationToken = default);

        Task<TransferResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
