# Persistence Strategy

## Repository Pattern

Each Aggregate has one Repository.

Examples

- IFinancialMovementRepository
- IHouseholdRepository
- IBudgetRepository

## Persistence Rules

- Save only Aggregate Roots.
- Never persist child entities independently.
- Reference Aggregates by identity.
- Use optimistic concurrency.
