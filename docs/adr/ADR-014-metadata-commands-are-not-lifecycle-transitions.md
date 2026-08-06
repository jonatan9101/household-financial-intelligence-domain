# ADR-014 — Metadata Commands Are Not Lifecycle Transitions

**Status:** Accepted

**Date:** 2026-08-05

## Context

During Phase 2 of the `FinancialAccount` aggregate, the `RenameFinancialAccount`
command revealed a documentation inconsistency:

- The lifecycle section listed `RenameFinancialAccount` under the `Active` state.
- The Business Rule Matrix and Command Traceability assigned only FA-006
  (AccountName required and non-blank) to `Rename`, with no status invariant.

Per the project governance, business rules must not be invented. Two options
were presented:

- **Option A:** `RenameFinancialAccount` is allowed in both `Active` and
  `Closed`. It is a metadata change, not a lifecycle transition.
- **Option B:** introduce a new rule (e.g. FA-011: Closed accounts cannot be
  renamed) and update all documentation.

Option A was approved. Renaming changes only metadata (`AccountName`); it does
not affect the lifecycle, the business identity, or any documented invariant.

## Decision

Within an Aggregate, two distinct command kinds exist and are modeled
differently:

1. **Lifecycle transitions** change the Aggregate status (e.g.
   `CloseFinancialAccount`: `Active -> Closed`, guarded by FA-007). They have
   explicit preconditions and postconditions and always publish a business
   event.

2. **Metadata commands** update a descriptive value without changing the
   lifecycle status (e.g. `RenameFinancialAccount`). Unless a documented rule
   explicitly restricts them by state, they are allowed in **all** lifecycle
   states and must not be listed as a state-specific transition.

Rules for the Domain Model:

- A command that does not change `Status` is a metadata command, not a lifecycle
  transition.
- Metadata commands must not be placed under a single state in the lifecycle
  documentation unless a business rule explicitly justifies a status guard.
- A status guard is introduced only when a documented invariant requires it,
  never to "protect" the lifecycle aesthetically.
- The Business Rule Matrix is the normative source: if no rule assigns a
  status precondition to a metadata command, none is implemented.

## Consequences

### Positive

- No speculative rules (no FA-011); documentation and code agree.
- Metadata editing works uniformly regardless of state, avoiding surprising
  `DomainException`s for operations that do not threaten invariants.
- The pattern is reusable for future Aggregates (e.g. renaming a `Budget` or a
  `FinancialGoal` while it is active, closed, or achieved).

### Negative

- A stakeholder who later wants to forbid renames in a given state must propose
  a new documented rule and update the Matrix, Specification, Behavior and
  Traceability documents.

## Related Documents

- ADR-013 — Behavior Must Be Rule Driven
- `docs/03-domain-model/household-finance/aggregates/financial-account/02-business-behavior.md`
- `docs/03-domain-model/household-finance/aggregates/financial-account/05-business-rule-matrix.md`
- `docs/03-domain-model/household-finance/aggregates/financial-account/06-command-traceability.md`
