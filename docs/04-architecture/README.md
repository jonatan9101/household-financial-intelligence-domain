# Architecture

This section describes how the HFI domain is implemented.

## Architectural Style

- Domain-Driven Design
- Clean Architecture
- CQRS
- Event-Driven Architecture
- Vertical Slice Architecture

## Principles

- Business rules remain inside the Domain layer.
- Each Aggregate defines a transaction boundary.
- Commands modify state.
- Queries never modify state.
- Read Models are projections.
- Infrastructure depends on the Domain, never the opposite.
