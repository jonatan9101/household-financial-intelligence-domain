# RegisterFinancialMovement

## Capability

Financial Acquisition

---

## Purpose

Registers a new immutable financial fact inside a Household.

A successful registration creates the Aggregate `FinancialMovement`
and publishes the corresponding Domain Events.

No financial calculations are performed during this use case.

Budgets, balances, cash flow and financial insights are projections
built afterwards.

---

## Primary Actor

Household Member

---

## Supporting Actors

Document Storage

Event Bus

Clock

Duplicate Detection Service

---

## Aggregate

FinancialMovement

---

## Trigger

A user manually registers a movement or a future ingestion process
creates the command automatically.

---

## Preconditions

- Household exists.
- Household is active.
- FinancialAccount exists.
- FinancialAccount belongs to Household.
- Amount is greater than zero.
- Currency is supported.
- TransactionDate is valid.
- User has permission.

---

## Postconditions

- FinancialMovement exists.
- FinancialMovement is immutable.
- FinancialMovementRegistered is published.
- Audit information is stored.

---

## Success Result

FinancialMovementId

---

## Failure Result

Business Error