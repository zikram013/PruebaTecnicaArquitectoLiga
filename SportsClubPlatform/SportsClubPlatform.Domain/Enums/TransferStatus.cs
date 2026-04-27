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

        BudgetValidationRequested = 2,
        BudgetValidated = 3,

        PlayerContractValidationRequested = 4,
        PlayerContractValidated = 5,

        PaymentRequested = 6,
        PaymentProcessed = 7,

        SquadUpdateRequested = 8,
        SquadsUpdated = 9,

        ContractGenerationRequested = 10,
        ContractGenerated = 11,

        NotificationRequested = 12,
        PartiesNotified = 13,

        Completed = 14,
        Failed = 15,
        CompensationRequested = 16,
        Compensated = 17
    }
}
