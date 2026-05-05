# PruebaTecnicaArquitectoLiga

Seven main bounded contexts have been identified: Squad Management, Transfer Management, Financial Management, Competition Management, Performance Analytics, Facilities Management and Fan Engagement. This separation allows business rules to be isolated, enables the most demanding capabilities to be scaled independently, and reduces coupling between domains with different rates of change.


The proposal adopts a hybrid architecture. Domains with high functional variability and independent scaling requirements, such as real-time events, analytics or the fan portal, are better suited to decoupled services. Conversely, highly transactional domains with strong business coherence, such as player transfers, squads and finance, can be based on a well-defined modular design, minimising unnecessary distributed complexity and facilitating progressive evolution.


The system is organised into capability-oriented containers. The API exposes operations to administrative and external clients; business services encapsulate each main domain; messaging decouples asynchronous processes and supports eventual consistency; and persistence is divided between operational storage and optimised query models.

Translated with DeepL.com (free version)

For the PoC, the transfer saga is implemented as an orchestrated workflow using MassTransit consumers and the Transfer aggregate as the persisted process state. This avoids adding EF-based saga persistence dependencies and keeps the PoC executable locally with SQLite. In a production scenario, the same flow could evolve to a MassTransit state machine saga with persistent saga repository and a durable message broker such as RabbitMQ or Azure Service Bus.

# Sports Club Platform - .NET Architecture Technical Test

## 1. Overview

This repository contains the proposed architecture and proof of concept for a SaaS platform designed to manage professional sports clubs globally.

The platform covers multiple business domains such as squad management, financial management, competitions, performance analytics, facilities, fan engagement, and player transfers.

The implemented Proof of Concept focuses on the **Transfer Management** domain and demonstrates an orchestrated player transfer workflow using ASP.NET Core, Entity Framework Core, SQLite, MassTransit, domain modeling, auditability, and compensating actions.

---

## 2. Technical Stack

- .NET 8
- C#
- ASP.NET Core Web API
- Entity Framework Core 8
- SQLite
- MassTransit with InMemory transport
- xUnit
- FluentAssertions
- Moq
- Swagger / OpenAPI

---

## 3. Solution Structure

```text
SportsClubPlatform.sln

src/
  SportsClubPlatform.Api
  SportsClubPlatform.Application
  SportsClubPlatform.Contracts
  SportsClubPlatform.Domain
  SportsClubPlatform.Infrastructure

tests/
  SportsClubPlatform.UnitTests

docs/
  architecture-notes.md
  architecture-decision.md
  bounded-contexts.md
  c4-model.md

## Architecture Documentation

Additional architecture documents are available in the `docs` folder:

| Document | Description |
|---|---|
| `architecture-notes.md` | Initial requirement analysis |
| `architecture-decision.md` | Architecture style decision |
| `bounded-contexts.md` | DDD bounded contexts |
| `context-map.md` | Context map between domains |
| `c4-context.md` | C4 system context diagram |
| `c4-container.md` | C4 container diagram |
| `c4-component-transfer.md` | C4 component diagram for Transfer Management |
| `transfer-flow.md` | Transfer orchestration sequence diagram |
| `testing-strategy.md` | Testing strategy |
| `architecture-summary.md` | Final architecture summary |



## Demo Endpoints

The following endpoints are useful for validating the PoC:

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/health` | Basic API health status |
| GET | `/api/clubs` | List seeded clubs and budgets |
| GET | `/api/players` | List seeded players |
| POST | `/api/transfers` | Submit a new transfer offer |
| GET | `/api/transfers/{id}` | Get transfer status |
| GET | `/api/transfers/{id}/audit` | Get transfer audit timeline |
