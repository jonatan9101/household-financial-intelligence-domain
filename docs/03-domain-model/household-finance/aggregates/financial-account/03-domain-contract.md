# Domain Contract

## Incoming Commands

- RegisterFinancialAccount
- RenameFinancialAccount
- CloseFinancialAccount
- ReopenFinancialAccount

## Published Events

- FinancialAccountRegistered
- FinancialAccountRenamed
- FinancialAccountClosed
- FinancialAccountReopened

## Read Models

- Household Accounts
- Active Accounts
- Account Summary

## Relationships

- Referenced by FinancialMovement using FinancialAccountId.
- Belongs to exactly one Household, referenced by identity (HouseholdId); authorization handled by the Household boundary.
- AccountIdentifier is the business identifier, distinct from FinancialAccountId; immutable once registered.