```md
# Transfer Orchestration Flow

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant TransferService
    participant Bus
    participant BudgetConsumer
    participant ContractConsumer
    participant PaymentConsumer
    participant SquadConsumer
    participant ContractGenerationConsumer
    participant NotificationConsumer
    participant CompensationConsumer
    participant DB

    Client->>API: POST /api/transfers
    API->>TransferService: SubmitTransferOfferAsync
    TransferService->>DB: Create Transfer
    TransferService->>Bus: Publish TransferOfferSubmitted
    API-->>Client: TransferResponse

    Bus->>BudgetConsumer: ValidateTransferBudget
    BudgetConsumer->>DB: Reserve budget
    BudgetConsumer->>Bus: TransferBudgetValidated

    Bus->>ContractConsumer: ValidatePlayerContract
    ContractConsumer->>DB: Validate active contract
    ContractConsumer->>Bus: PlayerContractValidated

    Bus->>PaymentConsumer: ProcessTransferPayment
    PaymentConsumer->>DB: Create Payment
    PaymentConsumer->>Bus: TransferPaymentProcessed

    Bus->>SquadConsumer: UpdateTransferSquads
    SquadConsumer->>DB: Move player to destination club
    SquadConsumer->>Bus: TransferSquadsUpdated

    Bus->>ContractGenerationConsumer: GenerateTransferContract
    ContractGenerationConsumer->>DB: Deactivate old contract and create new contract
    ContractGenerationConsumer->>Bus: TransferContractGenerated

    Bus->>NotificationConsumer: NotifyTransferParties
    NotificationConsumer->>DB: Create notification logs
    NotificationConsumer->>DB: Mark transfer as completed

    BudgetConsumer-->>CompensationConsumer: On failure
    ContractConsumer-->>CompensationConsumer: On failure
    PaymentConsumer-->>CompensationConsumer: On failure
    SquadConsumer-->>CompensationConsumer: On failure
    ContractGenerationConsumer-->>CompensationConsumer: On failure
    NotificationConsumer-->>CompensationConsumer: On failure

    CompensationConsumer->>DB: Release budget / revert player / cancel contract / mark payment compensated
