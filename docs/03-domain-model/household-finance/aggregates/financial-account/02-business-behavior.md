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
- RegisterFinancialAccount creates an Active account (there is no Draft stage).
- Closing an account preserves historical movements.
- AccountIdentifier is a business Value Object, distinct from FinancialAccountId.
- AccountStatus supports `Active` and `Closed`.

## Business Rules

- A FinancialAccount starts in Active when registered.
- AccountType is immutable once registered (FA-002).
- Currency is immutable once registered (FA-003).
- AccountIdentifier is immutable once registered (FA-005).
- AccountName is required and non-blank (FA-006).
- Only an Active account can be Closed (FA-007).
- Only a Closed account can be Reopened (FA-008).
- AccountIdentifier must be unique within the Household (FA-009).
- Closing an account never removes historical movements (FA-010).

## Lifecycle

Active &bull; Closed

- Active: RenameFinancialAccount
- Active -&gt; Closed: Close (only from Active; preserves historical movements)
- Closed -&gt; Active: Reopen (only from Closed)