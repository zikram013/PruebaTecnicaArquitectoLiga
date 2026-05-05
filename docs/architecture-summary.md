```md
# Architecture Summary

## Architecture Style

The proposed platform follows a hybrid architecture.

The overall system is decomposed into bounded contexts. Some domains can remain part of a modular core while others may evolve into independently deployable services depending on scalability, operational, and team ownership needs.

## Why Hybrid Architecture?

### Monolith Only
A pure monolith would be simple to build but would not fit well with:
- global multi-region requirements
- real-time event workloads
- high fan traffic
- independent scaling needs
- integration-heavy domains

### Microservices Only
A pure microservices approach would provide independent deployment and scalability but would introduce:
- distributed transaction complexity
- operational overhead
- deployment complexity
- more difficult debugging
- consistency challenges

### Hybrid
A hybrid architecture provides:
- strong modular boundaries
- progressive decomposition
- lower initial complexity
- independent scaling where needed
- event-driven integration between contexts

## PoC Architecture

The PoC implements Transfer Management inside a modular monolith using:
- ASP.NET Core Web API
- EF Core
- SQLite
- MassTransit InMemory transport
- asynchronous consumers
- audit entries
- compensating actions

## Saga Strategy

The transfer workflow is implemented as an orchestrated saga-like process.

The persisted `Transfer` aggregate stores the process state. Consumers move the transfer through the lifecycle and emit the next message.

This is intentionally simpler than a full MassTransit state machine saga with persistent saga repository, but it demonstrates the core architectural pattern clearly.

## Production Evolution

A production version should add:
- durable broker
- MassTransit state machine saga
- transactional outbox
- idempotency
- retries and dead-letter queues
- distributed tracing
- authentication and authorization
- observability
- multi-region deployment strategy
