# PruebaTecnicaArquitectoLiga

Seven main bounded contexts have been identified: Squad Management, Transfer Management, Financial Management, Competition Management, Performance Analytics, Facilities Management and Fan Engagement. This separation allows business rules to be isolated, enables the most demanding capabilities to be scaled independently, and reduces coupling between domains with different rates of change.


The proposal adopts a hybrid architecture. Domains with high functional variability and independent scaling requirements, such as real-time events, analytics or the fan portal, are better suited to decoupled services. Conversely, highly transactional domains with strong business coherence, such as player transfers, squads and finance, can be based on a well-defined modular design, minimising unnecessary distributed complexity and facilitating progressive evolution.


The system is organised into capability-oriented containers. The API exposes operations to administrative and external clients; business services encapsulate each main domain; messaging decouples asynchronous processes and supports eventual consistency; and persistence is divided between operational storage and optimised query models.

Translated with DeepL.com (free version)

For the PoC, the transfer saga is implemented as an orchestrated workflow using MassTransit consumers and the Transfer aggregate as the persisted process state. This avoids adding EF-based saga persistence dependencies and keeps the PoC executable locally with SQLite. In a production scenario, the same flow could evolve to a MassTransit state machine saga with persistent saga repository and a durable message broker such as RabbitMQ or Azure Service Bus.
