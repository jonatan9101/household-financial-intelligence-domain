# Aggregate Specification

## Aggregate Root

FinancialAccount

## Identity

FinancialAccountId

`AccountIdentifier` is the business identifier of the account. It is a distinct Value Object from `FinancialAccountId`.

- `FinancialAccountId` is the technical identity used to reference the account (e.g. by `FinancialMovement`).
- `AccountIdentifier` is a business Value Object (e.g. IBAN, account number) that can never change.

## Aggregate Boundary

Included:
- FinancialAccount

Excluded:
- Household
- FinancialMovement
- Budget
- FinancialGoal

`FinancialAccount` does not store balances. Balances are projections derived from `FinancialMovement`.

## Responsibilities

- Register financial accounts
- Maintain account metadata (e.g. RenameFinancialAccount, a metadata change available in both `Active` and `Closed`)
- Associate account to one Household
- Protect account lifecycle

## Invariants

- FA-001 Belongs to exactly one Household.
- FA-002 Account type cannot change after creation.
- FA-003 Currency cannot change.
- FA-004 Closed accounts cannot receive new movements.
- FA-005 AccountIdentifier (business identifier) is immutable.
- FA-006 AccountName is required and non-blank.
- FA-007 Only an Active account can be Closed.
- FA-008 Only a Closed account can be Reopened.
- FA-009 AccountIdentifier is unique within its Household.
- FA-010 Closing an account preserves historical movements.

## Value Objects

- AccountType
- Currency
- AccountName
- AccountIdentifier
- InstitutionName (optional)
- AccountStatus

## Consistency Boundary

One Aggregate.
One Transaction.
Cross-Aggregate consistency is achieved through Domain Events and identity reference.