# Test Specification

## Acceptance Criteria

### AC-001 Register Account

Given a valid Household
When RegisterFinancialAccount executes
Then an Active FinancialAccount is created.

### AC-002 Close Account

Given an active account
When CloseFinancialAccount executes
Then status becomes Closed.

### AC-003 Historical Integrity

Closing an account never removes existing FinancialMovements.

## Domain Tests

- Register account
- Reject duplicate identifier
- Close account
- Reopen account
