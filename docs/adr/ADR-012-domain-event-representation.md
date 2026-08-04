# ADR-012 — Domain Event Representation

**Status:** Accepted

**Date:** 2026-08-03

## Context

The `Household` aggregate publishes domain events that must be observable by the
Application layer after a command executes. Its `FinancialMovement` counterpart
published exactly one event type (`FinancialMovementRegistered`) and therefore
stored its events in a strongly typed collection:

```csharp
private readonly List<FinancialMovementRegistered> _domainEvents = [];
```

`Household` publishes more than one event type (`HouseholdCreated`,
`HouseholdActivated`, and, once implemented, `HouseholdArchived`). The collection
must be able to hold different record types.

Two options were available:

1. Introduce an abstraction such as `IDomainEvent` (or a common base type) that
   every event implements, and store `List<IDomainEvent>`.
2. Store the events as `List<object>`.

Option 1 was considered but rejected: with only three concrete event records and
no consumer that dispatches events through a shared contract, an interface adds
a type that exists solely to group other types. No method or component needs it
today. The MVP philosophy (ADR-002, ADR-009) rejects abstractions without a real
use case.

## Decision

Use `List<object>` for the aggregate's domain event collection.

```csharp
private readonly List<object> _domainEvents = [];

public IReadOnlyCollection<object> DomainEvents => _domainEvents;
```

There is no `IDomainEvent`. Consumers (currently only tests) retrieve events with
`OfType<T>()`:

```csharp
household.DomainEvents.OfType<HouseholdCreated>().Single();
```

Event records themselves remain strongly typed and immutable; only the holding
collection is untyped.

## Conditions That Should Revisit This Decision

The decision should be revisited — introducing an `IDomainEvent` contract — if
and when any of the following becomes real:

- A component must process events polymorphically without knowing their concrete
  type (e.g., an event dispatcher, outbox, or event store).
- An event handler must accept any event type through a single parameter.
- Persistence must serialize/deserialize events generically by contract rather
  than by concrete record.
- The number of event types grows to the point where `OfType<T>()` filtering
  becomes error-prone and a common shape would improve readability.

None of these conditions exist in the MVP. When one arises, the change is
contained: add `IDomainEvent` (empty contract or with shared members), make each
record implement it, and change `List<object>` to `List<IDomainEvent>`.

## Alternatives Considered

- `List<IDomainEvent>` — rejected, abstraction without a real consumer.
- Common abstract base record — rejected, same rationale as `IDomainEvent`.
- One strongly typed collection per event type — rejected, forces consumers to
  check multiple lists and complicates `ClearDomainEvents()`.

## Consequences

### Positive

- No new abstraction; the simplest representation that holds multiple event types.
- Adding a new event record is a one-line change in the aggregate.
- The contract stays aligned with YAGNI and KISS.

### Negative

- `DomainEvents` is typed as `object`, losing some compile-time type safety.
  Consumers must filter with `OfType<T>()`.
- The empty-`object` collection invites misuse if a consumer stores arbitrary
  objects; only aggregate code adds events, and it only adds event records.

## Related Documents

- ADR-002 — Capabilities over Epics
- ADR-009 — Business Before Technology
- `src/HouseholdFinancialIntelligence.Domain/Aggregates/Household/Household.cs`
- `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/FinancialMovement.cs`
