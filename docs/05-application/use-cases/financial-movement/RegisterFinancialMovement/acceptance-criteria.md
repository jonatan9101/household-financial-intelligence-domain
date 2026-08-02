# Acceptance Criteria

## Scenario 1

Given

An active Household

And an active FinancialAccount

When

RegisterFinancialMovement executes

Then

A FinancialMovement is persisted

And FinancialMovementRegistered is published.

---

Scenario 2

Given

The same movement already exists

When

The command executes

Then

DuplicateMovement is returned.

---

Scenario 3

Given

An inactive Household

When

The command executes

Then

HouseholdInactive is returned.

---

Scenario 4

Given

A closed FinancialAccount

When

The command executes

Then

FinancialAccountClosed is returned.