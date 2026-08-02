# Test Specification

## Acceptance Criteria

### AC-001 Create Goal

Given a valid Household
When CreateFinancialGoal executes
Then a Draft goal is created.

### AC-002 Complete Goal

Given an active goal
When CompleteFinancialGoal executes
Then the goal becomes Completed.

### AC-003 Prevent Modification

Given a completed goal
When UpdateFinancialGoal executes
Then the command is rejected.

## Domain Tests

- Create goal
- Update active goal
- Complete goal
- Cancel goal
- Reject updates to completed goal
