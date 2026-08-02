# RegisterFinancialMovementCommand

## Responsibility

Requests the registration of a new immutable FinancialMovement.

---

## Properties

HouseholdId

FinancialAccountId

Amount

Currency

MovementType

TransactionDate

Description

MerchantReference

EvidenceReference

CorrelationId

RequestedBy

OccurredAt

---

## Validation Level

Application Layer

No business rules.

Only syntactic validation.

---

## Idempotency

Supported.

Duplicate CorrelationId returns the previous result.