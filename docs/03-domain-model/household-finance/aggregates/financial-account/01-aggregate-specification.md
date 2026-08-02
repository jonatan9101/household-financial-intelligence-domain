# Aggregate Specification

## Aggregate Root

FinancialAccount

## Identity

FinancialAccountId

## Responsibilities

- Register financial accounts
- Maintain account metadata
- Associate account to one Household
- Protect account lifecycle

## Invariants

- FA-001 Belongs to exactly one Household.
- FA-002 Account type cannot change after creation.
- FA-003 Currency cannot change.
- FA-004 Closed accounts cannot receive new movements.
- FA-005 Account identifier is immutable.

## Value Objects

- AccountType
- Currency
- AccountName
- InstitutionName
- AccountStatus
