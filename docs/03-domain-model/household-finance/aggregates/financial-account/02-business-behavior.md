# Business Behavior

## Commands

- RegisterFinancialAccount
- RenameFinancialAccount
- CloseFinancialAccount
- ReopenFinancialAccount

## Domain Events

- FinancialAccountRegistered
- FinancialAccountRenamed
- FinancialAccountClosed
- FinancialAccountReopened

## Policies

- One account belongs to one Household.
- Closing an account preserves historical movements.

## Lifecycle

Draft -> Active -> Closed
