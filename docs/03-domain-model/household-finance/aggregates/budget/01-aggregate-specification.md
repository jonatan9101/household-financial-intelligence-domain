# Aggregate Specification

## Aggregate Root

Budget

## Identity

BudgetId

## Invariants

- BG-001 Budget belongs to exactly one Household.
- BG-002 Budget period cannot overlap another budget with the same scope.
- BG-003 Planned amount must be greater than zero.
- BG-004 Currency equals Household base currency.
- BG-005 Closed budgets cannot be modified.

## Value Objects

- BudgetPeriod
- PlannedAmount
- BudgetScope
- BudgetStatus
- Currency

## Consistency Boundary

The Budget aggregate protects planning consistency only.
Actual spending is computed outside this aggregate.
