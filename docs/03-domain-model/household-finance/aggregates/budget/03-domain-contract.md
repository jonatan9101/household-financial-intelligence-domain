# Domain Contract

## Incoming Commands

- CreateBudget
- UpdateBudget
- CloseBudget

## Published Events

- BudgetCreated
- BudgetUpdated
- BudgetClosed

## Read Models

- Budget Overview
- Budget vs Actual
- Budget History

## Relationships

References Household by HouseholdId.
Read models correlate Budget with FinancialMovement events.
