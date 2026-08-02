# Domain Contract

## Incoming Commands

- RegisterFinancialMovement
- AssignCategory
- InvalidateMovement

## Published Events

- FinancialMovementRegistered
- MovementPosted
- MovementCategorized
- MovementInvalidated

## Read Models

- Household Ledger
- Monthly Spending
- Spending by Category
- Cash Flow

## External Dependencies

- Household
- FinancialAccount
- FinancialDocument

Relationships are maintained through identifiers only.
