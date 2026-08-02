# Event-Driven Architecture

## Domain Events

Published only by Aggregates.

Examples

- FinancialMovementRegistered
- BudgetCreated
- HouseholdCreated

## Event Bus Responsibilities

- Publish events
- Retry failed deliveries
- Preserve ordering per Aggregate

## Integration Events

External systems consume Integration Events, not Domain Events directly.
