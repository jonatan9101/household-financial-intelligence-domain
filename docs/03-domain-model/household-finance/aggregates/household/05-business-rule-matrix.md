# Business Rule Matrix

Maps every Household business rule to the Aggregate methods that establish and preserve it.

Every rule has exactly one establishing method.

Every Aggregate method must justify its existence by protecting at least one business rule.

## Rules

| Rule   | Statement                                                                 | Established By        | Preserved By                                     |
| ------ | ------------------------------------------------------------------------- | --------------------- | ------------------------------------------------ |
| HH-001 | Household has exactly one Owner.                                          | Household.Create      | Household.RemoveMember (P-04), Household.ChangeMemberRole (P-05) |
| HH-002 | Every Member belongs to one Household.                                    | Household.AddMember   | Household.RemoveMember (members exist only inside the Aggregate) |
| HH-003 | BaseCurrency may be assigned or changed only while Draft; immutable once Active. | Household.SetBaseCurrency | Household.SetBaseCurrency (Draft-only guard), Household.Activate (freezes on Active) |
| HH-004 | TimeZone must always exist.                                               | Household.Create      | — (required invariant, no method clears it)      |
| HH-005 | Archived Households cannot accept new members.                            | Household.Archive     | Household.AddMember (rejects when Archived)      |
| HH-006 | HouseholdId is immutable.                                                 | Household.Create      | — (identity is never exposed for mutation)       |
| HH-007 | Activate() requires exactly one Owner and a defined BaseCurrency; otherwise activation fails. | Household.Activate | — (single transition)                            |
| P-01   | Only the Owner may archive a Household.                                   | Household.Archive     | — (enforced at archive)                          |
| P-02   | Removing a member never removes historical financial data.                | Household.RemoveMember | — (only membership is affected)                  |
| P-03   | Ownership transfer must preserve exactly one Owner.                       | Deferred — no method  | Deferred — no method (see TODO)                  |
| P-04   | The last Owner cannot be removed.                                         | Household.RemoveMember | — (enforced at removal)                          |
| P-05   | The last Owner cannot lose the Owner role.                                | Household.ChangeMemberRole | — (enforced at role change)                   |
| D3     | A Member has exactly one active role.                                     | Household.AddMember   | Household.ChangeMemberRole (replaces, never accumulates) |

## Methods

Every Aggregate method must justify its existence by protecting at least one business rule.

| Method                    | Protected Rules        | Justification                                                                 | Status      |
| ------------------------- | ---------------------- | ----------------------------------------------------------------------------- | ----------- |
| Household.Create          | HH-001, HH-004, HH-006 | Establishes the single Owner, the required TimeZone, and the immutable identity. | Implemented |
| Household.SetBaseCurrency | HH-003                 | The only place BaseCurrency can be assigned or changed, guarded by the Draft-only rule. | Implemented |
| Household.Activate        | HH-007                 | The only place activation can succeed or fail (requires Owner + BaseCurrency). | Implemented |
| Household.AddMember       | HH-002, HH-005, D3     | Adds a member only to an existing Household, only while not Archived, with exactly one active role. | Implemented |
| Household.RemoveMember    | P-02, P-04             | Removes membership without touching historical data and never removes the last Owner. | Implemented |
| Household.ChangeMemberRole | P-05, D3, HH-001 | Replaces the single active role, never removes the Owner role from the last Owner, and never creates a second Owner. | Implemented |
| Household.Archive         | P-01, HH-005           | The only method that can archive, guarded by the Owner rule.                  | Implemented |

## Notes

- P-03 (TransferOwnership) is deferred until a dedicated business capability is specified. It has no establishing or preserving method in the MVP scope.
- SetBaseCurrency publishes no event: BaseCurrency changes while Draft are internal Aggregate state.
