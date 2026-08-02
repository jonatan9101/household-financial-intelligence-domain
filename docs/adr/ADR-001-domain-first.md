# ADR-005 — Small Aggregates

**Status:** Accepted

**Date:** 2026-08-01

## Context

Large aggregates reduce scalability.

## Decision

Each aggregate protects only its own invariants.

## Alternatives Considered

- Technology First
- Data Model First
- Domain First (Selected)

## Consequences

### Positive

- Better scalability
- Clear transactions

### Negative

- Requires eventual consistency

## Related Documents

- Domain Principles
- Domain Vision
- Strategic Design
