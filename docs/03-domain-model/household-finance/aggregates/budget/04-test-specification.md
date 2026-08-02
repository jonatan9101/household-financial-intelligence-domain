# Test Specification

## Acceptance Criteria

### AC-001 Create Budget
Given a valid Household
When CreateBudget executes
Then a Draft Budget is created.

### AC-002 Prevent Overlap
Given an active budget for the same scope and period
When CreateBudget executes
Then the command is rejected.

### AC-003 Close Budget
Given an active budget
When CloseBudget executes
Then it becomes immutable.

## Domain Tests

- Create budget
- Reject overlapping budgets
- Update draft budget
- Close budget
- Prevent updates after closure
