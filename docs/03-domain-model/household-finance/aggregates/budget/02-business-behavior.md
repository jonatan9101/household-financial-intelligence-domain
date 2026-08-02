# Business Behavior

## Commands

- CreateBudget
- UpdateBudget
- CloseBudget
- ReopenBudget

## Domain Events

- BudgetCreated
- BudgetUpdated
- BudgetClosed
- BudgetReopened

## Policies

- Only one active budget per scope and period.
- Closed budgets are immutable.
- Budget revisions create audit history.

## Lifecycle

Draft -> Active -> Closed
