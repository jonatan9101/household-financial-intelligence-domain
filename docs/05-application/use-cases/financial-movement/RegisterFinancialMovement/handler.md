# RegisterFinancialMovementHandler

## Responsibilities

Validate command.

Load Household.

Load FinancialAccount.

Detect duplicates.

Invoke Aggregate.

Persist Aggregate.

Commit transaction.

Publish Domain Events.

Return FinancialMovementId.

---

## Transaction Boundary

FinancialMovement Aggregate.

---

## Dependencies

Clock

UnitOfWork

FinancialMovementRepository

HouseholdRepository

FinancialAccountRepository

DuplicateDetectionService

EventBus

---

## Does NOT

Calculate balances.

Update budgets.

Create projections.

Send notifications.