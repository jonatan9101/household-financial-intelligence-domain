# Test Specification

## Acceptance Criteria

### AC-001 Register Account

Given a valid Household
When RegisterFinancialAccount executes with a unique AccountIdentifier
Then an Active FinancialAccount is created.

### AC-002 Register Account Rejects Duplicate Identifier

Given a Household that already has an account with a given AccountIdentifier
When RegisterFinancialAccount executes with the same AccountIdentifier
Then registration is rejected (FA-009).

### AC-003 Register Account Validates Name

Given a blank AccountName
When RegisterFinancialAccount executes
Then registration is rejected (FA-006).

### AC-004 Close Account

Given an Active account
When CloseFinancialAccount executes
Then status becomes Closed (FA-007).

### AC-005 Close Rejects Non-Active

Given a Closed account
When CloseFinancialAccount executes
Then the action is rejected (FA-007).

### AC-006 Reopen Account

Given a Closed account
When ReopenFinancialAccount executes
Then status becomes Active (FA-008).

### AC-007 Reopen Rejects Non-Closed

Given an Active account
When ReopenFinancialAccount executes
Then the action is rejected (FA-008).

### AC-008 Historical Integrity

Closing an account never removes existing FinancialMovements (FA-010).

### AC-009 Rename Account

Given an account with an existing name
When RenameFinancialAccount executes with a non-blank name
Then the AccountName is updated (FA-006).

### AC-010 Rename Rejects BlankName

Given an account
When RenameFinancialAccount executes with a blank name
Then the action is rejected (FA-006).

## Domain Tests

- Register account
- Reject duplicate AccountIdentifier
- Reject blank AccountName on register
- Close account
- Reopen account
- Reject closing a Closed account
- Reject reopening an Active account
- Rename account
- Reject blank name on rename
- Immutability of AccountType, Currency, AccountIdentifier