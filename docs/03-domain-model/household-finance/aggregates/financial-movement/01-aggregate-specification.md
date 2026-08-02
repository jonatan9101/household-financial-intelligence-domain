# Aggregate Specification

## Purpose

Protect the consistency boundary of a single financial movement.

## Aggregate Root

FinancialMovement

## Aggregate Boundary

Included:
- FinancialMovement

Excluded:
- Household
- FinancialAccount
- Budget
- FinancialGoal
- Merchant
- Category
- Interpretation

## Identity

FinancialMovementId

## Invariants

- FM-001 Every FinancialMovement belongs to one Household.
- FM-002 Every FinancialMovement belongs to one FinancialAccount.
- FM-003 Amount is immutable.
- FM-004 Currency is immutable.
- FM-005 TransactionDate is immutable.
- FM-006 Original evidence is immutable.
- FM-007 Facts never change.
- FM-008 Interpretations never modify facts.

## Value Objects

- Money
- Currency
- MovementType
- MovementStatus
- TransactionDate
- MerchantReference
- EvidenceReference

## Consistency Boundary

One Aggregate.
One Transaction.
Cross-Aggregate consistency is achieved through Domain Events.
