# ADR-006 — Reference Aggregates by Identity

**Status:** Accepted

**Date:** 2026-08-01

## Context

Direct object references create coupling between aggregates.

## Decision

Aggregates reference other aggregates exclusively through identifiers.

## Alternatives Considered

- Tighter runtime coupling
- Shared database
- Selected approach

## Consequences

### Positive

- Smaller aggregate boundaries
- Independent lifecycle
- Better scalability

### Negative

- More repository lookups in application layer

## Related Documents

- Domain Principles
- Strategic Design
- Tactical Design
