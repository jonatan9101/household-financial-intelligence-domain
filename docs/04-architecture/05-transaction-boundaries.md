# Transaction Boundaries

## Rule

One Aggregate = One Transaction

Cross-Aggregate consistency is achieved through Domain Events.

## Examples

RegisterFinancialMovement
- Transaction: FinancialMovement

CreateBudget
- Transaction: Budget

CreateHousehold
- Transaction: Household
