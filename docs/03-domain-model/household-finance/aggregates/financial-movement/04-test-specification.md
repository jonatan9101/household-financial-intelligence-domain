# Test Specification

## Acceptance Criteria

### AC-001 Register Movement

Given a valid financial document
When RegisterFinancialMovement is executed
Then a FinancialMovement is created.

### AC-002 Duplicate Detection

Given the same document is processed twice
When RegisterFinancialMovement is executed
Then only one FinancialMovement exists.

### AC-003 Category Assignment

Given a registered movement
When AssignCategory is executed
Then only the interpretation changes.

### AC-004 Invalidation

Given a posted movement
When InvalidateMovement is executed
Then the movement becomes Invalidated while preserving evidence.

## Domain Test Scenarios

- Register a valid movement.
- Reject invalid data.
- Ignore duplicate imports.
- Preserve immutable facts.
- Publish expected domain events.
