# ADR-008 — Command Query Responsibility Segregation

**Status:** Accepted

**Date:** 2026-08-01

## Context

Read and write models have different optimization goals.

## Decision

Separate write-side consistency from read-side projections.

## Alternatives Considered

- Tighter runtime coupling
- Shared database
- Selected approach

## Consequences

### Positive

- Optimized queries
- Simpler aggregates

### Negative

- Additional projection infrastructure

## Related Documents

- Domain Principles
- Strategic Design
- Tactical Design
