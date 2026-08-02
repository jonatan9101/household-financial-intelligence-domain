# Business Behavior

## Commands

- CreateFinancialGoal
- UpdateFinancialGoal
- CompleteFinancialGoal
- CancelFinancialGoal
- ReopenFinancialGoal

## Domain Events

- FinancialGoalCreated
- FinancialGoalUpdated
- FinancialGoalCompleted
- FinancialGoalCancelled
- FinancialGoalReopened

## Policies

- Only active goals may be updated.
- Completed goals are immutable.
- Cancelled goals preserve history.

## Lifecycle

Draft -> Active -> Completed
              |
              +-> Cancelled
