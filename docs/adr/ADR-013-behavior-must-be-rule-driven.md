# ADR-013 — Behavior Must Be Rule Driven

**Status:** Accepted

**Date:** 2026-08-03

## Context

During Phase 2 of the `Household` aggregate, the command `RenameHousehold` was
candidate for implementation. It was removed from the command list on review:
no documented business rule (HH-001…HH-007, P-01…P-05, or D3) was protected by
renaming the Household. Attributing it to HH-006 (immutable identity) was
revealed as a forced justification, since HH-006 is established by `Create` and
preserved structurally by the aggregate's get-only identity, not by renaming.

A review of the candidate exposed the temptation to add commands because a
property exists ("rename the name") rather than because a business rule demands
it. That temptation is a persistence/CRUD or UI reflex, not a Domain concern.

## Decision

Aggregate commands emerge from business rules. A command exists **only if** it
protects one or more documented business rules.

Rules for the Domain Model:

- A command is never implemented merely to change a mutable property.
- A command is never added because a user interface or a CRUD endpoint would
  expose it.
- A command is never a placeholder or a TODO: it either protects a rule and
  belongs to the Aggregate, or it does not exist.
- If a command cannot be mapped to a documented business rule, it is removed
  from the Aggregate and from the specification, not deferred.

`RenameHousehold` was removed from `Household` for this reason.

## Consequences

### Positive

- The aggregate surface is the direct, auditable trace of protected business
  rules (see the Business Rule Matrix).
- No dead or speculative behavior; YAGNI is enforced at the model level, not
  only at implementation time.
- Reviews focus on business justification rather than implementation detail.

### Negative

- Business experts must agree that a behavior exists before it is modeled; a
  stakeholder who "wants a rename button" must articulate the rule it protects.

## Related Documents

- ADR-002 — Capabilities over Epics
- ADR-009 — Business Before Technology
- `docs/03-domain-model/household-finance/aggregates/household/05-business-rule-matrix.md`
- `docs/03-domain-model/household-finance/aggregates/household/02-business-behavior.md`