# Command Traceability — FinancialAccount

Tracks, per implemented command, which business rules it enforces and which tests cover them.

> Status: Phase 1 (documentation) complete. Implementation and test coverage are recorded here during Phase 2.

## RegisterFinancialAccount()

| Command                 | Rules Enforced |
| ----------------------- | -------------- |
| RegisterFinancialAccount() | FA-001 (one Household), FA-002 (type immutable), FA-003 (currency immutable), FA-005 (identifier immutable), FA-006 (name required), FA-009 (identifier unique), FA-010 (preserves movements) |

## RenameFinancialAccount()

| Command             | Rules Enforced |
| -------------------- | -------------- |
| RenameFinancialAccount() | FA-006 (name required and non-blank) |

## CloseFinancialAccount()

| Command               | Rules Enforced |
| ---------------------- | -------------- |
| CloseFinancialAccount() | FA-004 (Closed accounts reject new movements), FA-007 (only Active can close), FA-010 (preserves movements) |

## ReopenFinancialAccount()

| Command               | Rules Enforced |
| ---------------------- | -------------- |
| ReopenFinancialAccount() | FA-008 (only Closed can reopen) |

## Acceptance Criteria Mapping — Pending

| AC   | Rule                          | Test Name (planned)                                    |
| ---- | ----------------------------- | ------------------------------------------------------- |
| AC-001 | FA-006, FA-009              | Given_ValidFacts_When_Registering_Then_AccountIsActive |
| AC-002 | FA-009                        | Given_DuplicateIdentifier_When_Registering_Then_Rejected |
| AC-003 | FA-006                        | Given_BlankName_When_Registering_Then_Rejected         |
| AC-004 | FA-007                        | Given_ActiveAccount_When_Closing_Then_StatusBecomesClosed |
| AC-005 | FA-007                        | Given_ClosedAccount_When_Closing_Then_Rejected         |
| AC-006 | FA-008                        | Given_ClosedAccount_When_Reopening_Then_StatusBecomesActive |
| AC-007 | FA-008                        | Given_ActiveAccount_When_Reopening_Then_Rejected       |
| AC-008 | FA-010                        | Given_Closing_Then_MovementsArePreserved               |
| AC-009 | FA-006                        | Given_Renaming_Then_NameIsUpdated                      |
| AC-010 | FA-006                        | Given_BlankName_When_Renaming_Then_Rejected            |

> Test names above are the target. Final names are recorded in Phase 2 as each command is implemented.