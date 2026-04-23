using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Domain.Enums
{
    /// <summary>
    /// Represents the current lifecycle status of a transfer process.
    /// </summary>
    public enum TransferStatus
    {
        Draft = 0,
        OfferSubmitted = 1,
        BudgetValidated = 2,
        PlayerContractValidated = 3,
        PaymentProcessed = 4,
        SquadsUpdated = 5,
        ContractGenerated = 6,
        Completed = 7,
        Failed = 8,
        Compensated = 9
    }
}
