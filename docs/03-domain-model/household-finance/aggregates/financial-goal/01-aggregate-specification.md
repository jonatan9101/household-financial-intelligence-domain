# Aggregate Specification

## Aggregate Root

FinancialGoal

## Identity

FinancialGoalId

## Invariants

- FG-001 Belongs to exactly one Household.
- FG-002 TargetAmount must be greater than zero.
- FG-003 TargetDate must be after creation date.
- FG-004 Currency equals Household base currency.
- FG-005 Completed or Cancelled goals cannot be modified.

## Value Objects

- GoalName
- GoalStatus
- TargetAmount
- TargetDate
- Currency
- GoalPriority

## Consistency Boundary

The aggregate protects the definition and lifecycle of a goal.
Goal progress is computed by projections.
