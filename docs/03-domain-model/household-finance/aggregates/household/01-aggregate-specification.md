# Aggregate Specification

## Aggregate Root

Household

## Identity

HouseholdId

## Invariants

- HH-001 Household has exactly one Owner.
- HH-002 Every Member belongs to one Household.
- HH-003 BaseCurrency is immutable after activation.
- HH-004 TimeZone must always exist.
- HH-005 Archived Households cannot accept new members.
- HH-006 HouseholdId is immutable.

## Value Objects

- HouseholdName
- BaseCurrency
- TimeZone
- Locale
- MemberRole
- HouseholdStatus

## Responsibilities

- Protect membership consistency
- Protect ownership
- Publish household lifecycle events
