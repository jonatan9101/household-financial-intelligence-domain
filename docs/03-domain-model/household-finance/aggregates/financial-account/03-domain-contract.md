# Domain Contract

## Incoming Commands

- RegisterFinancialAccount
- RenameFinancialAccount
- CloseFinancialAccount

## Published Events

- FinancialAccountRegistered
- FinancialAccountClosed

## Read Models

- Household Accounts
- Active Accounts
- Account Summary

## Relationships

Referenced by FinancialMovement using FinancialAccountId.
