# ADR-011 — EF Core Owned Type Materialization for the Aggregate

**Status:** Accepted

**Date:** 2026-08-02

## Context

The `FinancialMovement` aggregate is immutable by design: a single private
constructor, get-only properties, and a static factory (`Register`) that
validates and constructs the aggregate. Its `Amount` is a `Money` value object
(`decimal` + `Currency`), which the persistence mapping must store as two real
columns (`Amount` numeric(18,2), `CurrencyCode` varchar(3)).

EF Core 10 cannot materialize this aggregate. Its constructor binding resolves
only *scalar* mapped properties; navigations, including references to owned
types, cannot be bound to constructor parameters. With the sole constructor
taking a `Money amount` parameter, EF Core fails with:

```
Cannot bind 'amount' in 'FinancialMovement(... Money amount ...)'
Note that only mapped properties can be bound to constructor parameters.
Navigations to related entities, including references to owned types, cannot be bound.
```

The limitation is real, not a mapping error: it reproduces with both `OwnsOne`
(owned types) and `ComplexProperty` (complex types). It is also not fixable
with a private setter or a parameterless constructor without breaking other
constraints:

- A `private set` on `Amount` makes the property writable via reflection
  (`CanWrite == true`), violating the aggregate's tested immutability.
- A parameterless constructor would leave `Id` (get-only on `Entity<TId>`)
  without a materialization path, forcing a change to the base type.

## Decision

Keep the Domain immutable and adapt Infrastructure to it.

In `FinancialMovement`:

- `Amount` is removed from the private constructor.
- The get-only `public Money Amount => _amount;` is preserved.
- `_amount` is a private backing field (`private Money _amount = default!;`).
- The `Register` factory assigns `_amount` immediately after construction, so
  the aggregate is never exposed with an unset `Amount` and the setter is never
  reachable from outside the class.

In `FinancialMovementConfiguration`:

- `Money` is mapped with `OwnsOne` to two columns (`Amount`, `CurrencyCode`).
- The navigation is configured with
  `HasField("_amount")` + `UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)`
  so EF Core injects the materialized value into the backing field.

`Money`, `Currency`, `Entity<TId>`, and `AggregateRoot<TId>` are unchanged.

## Alternatives Considered

- Owned type bound to a constructor parameter — rejected, EF Core cannot bind
  owned/complex navigations to constructor parameters (proven with `OwnsOne`
  and `ComplexProperty`).
- `private set` on `Amount` — rejected, breaks the aggregate's immutability
  contract (`CanWrite == false`) enforced by the test suite.
- Parameterless EF constructor — rejected, would force `Entity<TId>.Id` to
  gain a write path, expanding the Domain change beyond the aggregate.
- Postgres composite type / JSONB column — rejected, couples the Domain to a
  persistence-specific representation.
- Selected approach: private backing field + field injection.

## Consequences

### Positive

- The Domain stays immutable; no public API or invariant changes.
- Infrastructure adapts to the Domain, not the reverse.
- `Money` persists as two real columns, keeping the schema explicit.

### Negative

- The aggregate carries a `default!` backing field that is only meaningful to
  EF Core materialization; the source of truth for correctness is the
  `Register` factory, which always assigns it.
- The mapping must stay in sync with the field name (`_amount`); renaming
  requires updating `FinancialMovementConfiguration`.

## Related Documents

- ADR-003 — Immutable Financial Facts
- ADR-009 — Business Before Technology
- `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/FinancialMovement.cs`
- `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/Configurations/FinancialMovementConfiguration.cs`
- `IMPLEMENTATION_PLAN.md` (M4)
