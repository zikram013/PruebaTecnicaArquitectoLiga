```md
# C4 Component Diagram - Transfer Management

```mermaid
flowchart TB
    Controller[TransfersController]
    AppService[TransferApplicationService]
    DbContext[AppDbContext]

    OfferConsumer[TransferOfferSubmittedConsumer]
    BudgetConsumer[ValidateTransferBudgetConsumer]
    ContractConsumer[ValidatePlayerContractConsumer]
    PaymentConsumer[ProcessTransferPaymentConsumer]
    SquadConsumer[UpdateTransferSquadsConsumer]
    GenerateContractConsumer[GenerateTransferContractConsumer]
    NotifyConsumer[NotifyTransferPartiesConsumer]
    CompensationConsumer[CompensateTransferConsumer]

    AuditService[TransferAuditService]
    Bus[MassTransit InMemory Bus]
    Db[(SQLite Database)]

    Controller --> AppService
    AppService --> DbContext
    AppService --> AuditService
    AppService --> Bus

    Bus --> OfferConsumer
    OfferConsumer --> BudgetConsumer
    BudgetConsumer --> ContractConsumer
    ContractConsumer --> PaymentConsumer
    PaymentConsumer --> SquadConsumer
    SquadConsumer --> GenerateContractConsumer
    GenerateContractConsumer --> NotifyConsumer

    BudgetConsumer --> CompensationConsumer
    ContractConsumer --> CompensationConsumer
    PaymentConsumer --> CompensationConsumer
    SquadConsumer --> CompensationConsumer
    GenerateContractConsumer --> CompensationConsumer
    NotifyConsumer --> CompensationConsumer

    OfferConsumer --> DbContext
    BudgetConsumer --> DbContext
    ContractConsumer --> DbContext
    PaymentConsumer --> DbContext
    SquadConsumer --> DbContext
    GenerateContractConsumer --> DbContext
    NotifyConsumer --> DbContext
    CompensationConsumer --> DbContext

    AuditService --> DbContext
    DbContext --> Db
