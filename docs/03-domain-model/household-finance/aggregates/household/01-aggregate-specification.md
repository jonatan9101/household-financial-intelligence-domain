# Aggregate Specification

## Aggregate Root

Household

## Identity

HouseholdId

## Invariants

- HH-001 Household has exactly one Owner.
- HH-002 Every Member belongs to one Household.
- HH-003 BaseCurrency may be assigned or changed only while the Household is Draft; once Active, BaseCurrency is immutable.
- HH-004 TimeZone must always exist.
- HH-005 Archived Households cannot accept new members.
- HH-006 HouseholdId is immutable.
- HH-007 Activate() requires exactly one Owner and a defined BaseCurrency; otherwise activation fails.

## Entities

- Member (child entity)
  - Belongs to exactly one Household.
  - Has exactly one active MemberRole.
  - Owner is a MemberRole, not a separate flag.

## Value Objects

- HouseholdName
- BaseCurrency
- TimeZone
- Locale
- MemberRole (Value Object, not an enum)
- HouseholdStatus

## Lifecycle

- Draft -> Active -> Archived
- SetBaseCurrency assigns or changes BaseCurrency while Draft (HH-003).
- Activate transitions Draft -> Active and requires exactly one Owner and a defined BaseCurrency (HH-007).
- Archive transitions Active -> Archived and requires the Owner (P-01).

## Responsibilities

- Protect membership consistency
- Protect ownership
- Publish household lifecycle events
- Guard base currency immutability
