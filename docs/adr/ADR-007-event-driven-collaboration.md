# ADR-007 — Event-Driven Collaboration

**Status:** Accepted

**Date:** 2026-08-01

## Context

Bounded Contexts need to collaborate without synchronous dependencies.

## Decision

Contexts communicate through immutable Domain Events.

## Alternatives Considered

- Tighter runtime coupling
- Shared database
- Selected approach

## Consequences

### Positive

- Loose coupling
- Independent deployment
- Natural extensibility

### Negative

- Eventual consistency

## Related Documents

- Domain Principles
- Strategic Design
- Tactical Design
