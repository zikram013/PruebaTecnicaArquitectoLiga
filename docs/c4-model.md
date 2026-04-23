# C4 Model Notes

## Context Diagram

### Main System
Sports Club Management Platform

### Actors
- Club Administrator
- Sporting Director
- Finance Officer
- Coach / Staff
- Fan

### External Systems
- SAP
- Oracle
- Payment Provider
- Notification Provider
- Identity Provider
- Regulatory / Federation Systems

## Container Diagram

### Containers
- Admin Web API
- Transfer Management Module
- Squad Management Module
- Financial Management Module
- Competition Management Module
- Fan Engagement Module
- Message Bus
- Operational Database
- Reporting / Read Models
- Notification Adapter

## PoC-Focused Containers
For the technical PoC, the implementation will focus on:
- ASP.NET Core API
- Transfer Orchestration Application Layer
- EF Core SQLite Database
- MassTransit InMemory Bus
- Notification Service Adapter
- Payment Service Adapter
