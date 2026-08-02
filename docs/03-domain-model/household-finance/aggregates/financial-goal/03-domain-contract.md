# Domain Contract

## Incoming Commands

- CreateFinancialGoal
- UpdateFinancialGoal
- CompleteFinancialGoal
- CancelFinancialGoal

## Published Events

- FinancialGoalCreated
- FinancialGoalCompleted
- FinancialGoalCancelled

## Read Models

- Goal Dashboard
- Goal Progress
- Household Goals

## Relationships

References Household using HouseholdId.
Progress is correlated with FinancialMovement projections.
