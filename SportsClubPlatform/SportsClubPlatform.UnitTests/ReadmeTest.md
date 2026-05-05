# Testing Strategy

## Goal

The goal of the test suite is to validate business rules and critical transfer workflow behavior.

## Test Levels

### Domain Tests
Validate pure business rules without infrastructure dependencies.

Covered examples:
- budget reservation
- budget release
- transfer creation invariants
- transfer status transitions
- player contract validity

### Application Tests
Validate application service behavior using an in-memory database.

Covered examples:
- transfer offer creation
- validation of missing player
- validation of missing destination club
- audit entry creation
- transfer offer event publication

### Consumer Tests
Validate selected MassTransit consumers in isolation.

Covered examples:
- budget validation success
- budget validation failure
- transfer status update
- audit entry creation

## Out of Scope for PoC

- full end-to-end distributed tests
- broker-backed integration tests
- concurrency tests
- load tests
- contract testing
- security tests