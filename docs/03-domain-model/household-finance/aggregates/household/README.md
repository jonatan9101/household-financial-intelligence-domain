# Household

> Aggregate Root representing the family boundary.

## Purpose

Household defines the ownership boundary for every financial fact in HFI.

Every FinancialMovement, FinancialAccount, Budget and FinancialGoal belongs to exactly one Household.

## Responsibilities

- Manage household lifecycle (Draft -> Active -> Archived)
- Manage members and roles (exactly one active role per Member)
- Maintain base currency (assigned in Draft, immutable once Active)
- Maintain timezone and locale
- Define ownership boundary

## Out of Scope

- Financial calculations
- Budget execution
- Reporting
- Recommendations
