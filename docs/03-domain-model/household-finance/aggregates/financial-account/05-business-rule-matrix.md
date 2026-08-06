# Business Rule Matrix

Maps every FinancialAccount business rule to the Aggregate methods that establish and preserve it.

Every rule has exactly one establishing method.

Every Aggregate method must justify its existence by protecting at least one rule.

## Rules

| Rule   | Statement                                                                   | Established By             | Preserved By                                    |
| ------ | --------------------------------------------------------------------------- | -------------------------- | ------------------------------------------------ |
| FA-001 | Belongs to exactly one Household.                                           | FinancialAccount.Register   | — (reference set once)                          |
| FA-002 | AccountType cannot change.                                                  | FinancialAccount.Register   | — (no mutating method)                          |
| FA-003 | Currency cannot change.                                                     | FinancialAccount.Register   | — (no mutating method)                          |
| FA-004 | Closed accounts cannot receive new movements.                               | FinancialAccount.Close      | Cross-aggregate check in RegisterFinancialMovement |
| FA-005 | AccountIdentifier (business identifier) is immutable.                       | FinancialAccount.Register   | — (no mutating method)                          |
| FA-006 | AccountName is required and non-blank.                                      | FinancialAccount.Register   | FinancialAccount.Rename                        |
| FA-007 | Only an Active account can be Closed.                                       | FinancialAccount.Close      | — (enforced at close)                          |
| FA-008 | Only a Closed account can be Reopened.                                      | FinancialAccount.Reopen     | — (enforced at reopen)                         |
| FA-009 | AccountIdentifier is unique within its Household.                           | FinancialAccount.Register   | — (enforced at registration)                   |
| FA-010 | Closing an account preserves historical movements.                          | FinancialAccount.Register   | — (movements are a separate Aggregate)         |

## Methods

| Method                     | Protected Rules        | Justification                                                                   | Status  |
| -------------------------- | ---------------------- | ------------------------------------------------------------------------------- | ------- |
| FinancialAccount.Register  | FA-001..003, FA-005, FA-006, FA-009, FA-010 | Creates an Active FinancialAccount with immutable metadata, immutable unique AccountIdentifier, and a required non-blank AccountName. | Implemented |
| FinancialAccount.Rename    | FA-006                 | Updates AccountName while keeping it required and non-blank.                   | Implemented |
| FinancialAccount.Close     | FA-004, FA-007, FA-010 | Moves Active to Closed only, preserving historical movements.                 | Implemented |
| FinancialAccount.Reopen    | FA-008                 | Moves Closed to Active only.                                                    | Implemented |

## Notes

- `AccountIdentifier` is a business Value Object distinct from `FinancialAccountId`. `FinancialAccountId` is the technical identity used for references; `AccountIdentifier` is what a business expert identifies the account with.
- `InstitutionName` is optional (may be absent).
- No Draft stage. RegisterFinancialAccount creates the account directly in Active.