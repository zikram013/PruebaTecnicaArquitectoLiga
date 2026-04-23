# Sports Club Platform - Architecture Notes

## 1. Goal
Design a SaaS platform for managing professional sports clubs globally, covering squad management, finances, competitions, performance analytics, facilities, and fan engagement.

## 2. Key Non-Functional Requirements
- 500+ clubs simultaneously
- 50,000 concurrent users
- Multi-region (Europe, America, Asia)
- 99.9% availability
- GDPR and sports regulations compliance
- Legacy integration (SAP, Oracle)
- Real-time event processing

## 3. Main Deliverables
- C4 Context diagram
- C4 Container diagram
- C4 Component diagram for one critical service
- Bounded contexts
- Context map
- Architecture style justification
- PoC for transfer orchestration using saga

## 4. Selected Critical Service
Transfer Management / Player Transfer Orchestration

## 5. Initial Architecture Decision
Hybrid architecture:
- modular core for transactional domains
- event-driven services for real-time and fan-facing capabilities

## 6. PoC Scope
The PoC will focus on transfer orchestration with:
- ASP.NET Core 8
- C#
- EF Core
- SQLite
- MassTransit
- Unit tests
- Seed data
