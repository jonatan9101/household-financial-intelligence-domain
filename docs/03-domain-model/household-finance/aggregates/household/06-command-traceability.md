# Command Traceability — Household

Tracks, per implemented command, which business rules it enforces and which tests cover them.

Updated after each Phase 2 command.

## Business Rule Coverage

### Create() — implemented

| Command    | Rules Enforced                                                                 |
| ---------- | ------------------------------------------------------------------------------ |
| Create()   | HH-001 (exactly one Owner), HH-002 (Owner belongs to the Household), HH-004 (TimeZone required), HH-006 (HouseholdId immutable) |

| Business Rule | Test Coverage                                                                  |
| ------------- | ------------------------------------------------------------------------------ |
| HH-001        | `Given_ValidFacts_When_Creating_Then_ExactlyOneOwnerExists`                    |
| HH-002        | `Given_ValidFacts_When_Creating_Then_ExactlyOneOwnerExists` (member belongs)   |
| HH-004        | `Given_BlankTimeZone_When_Creating_Then_DomainExceptionIsThrown`, `Given_ValidFacts_When_Creating_Then_AggregateReflectsThoseFacts` |
| HH-006        | `Given_CreatedHousehold_When_InspectingFacts_Then_TheyAreImmutable`, `Given_NewHouseholdIds_When_Creating_Then_TheyAreUnique` |
| Creation       | `Given_ValidCreation_When_Creating_Then_ExactlyOneHouseholdCreatedEventIsPublished` |

### SetBaseCurrency() — implemented

| Command          | Rules Enforced |
| ---------------- | -------------- |
| SetBaseCurrency() | HH-003 (BaseCurrency may be assigned or changed only while Draft; immutable once Active; Archived cannot change it) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| HH-003 (Draft)        | `Given_DraftHousehold_When_SetBaseCurrency_Then_BaseCurrencyIsSet`, `Given_DraftHousehold_When_ChangingBaseCurrency_Then_ItIsOverwritten` |
| HH-003 (reject Active) | `Given_ActiveHousehold_When_SetBaseCurrency_Then_DomainExceptionIsThrown` |
| HH-003 (reject Archived) | `Given_ArchivedHousehold_When_SetBaseCurrency_Then_DomainExceptionIsThrown` |
| HH-003 (valid currency) | `Given_DraftHousehold_When_SetBaseCurrency_WithInvalidCurrency_Then_DomainExceptionIsThrown` |
| No Domain Event | `Given_DraftHousehold_When_SetBaseCurrency_Then_NoDomainEventIsPublished` |

### Activate() — implemented

| Command    | Rules Enforced |
| ---------- | -------------- |
| Activate() | HH-007 (Activate requires exactly one Owner and a defined BaseCurrency; otherwise fails), lifecycle integrity (Draft -> Active only) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| HH-007 (success) | `Given_DraftHousehold_WithBaseCurrency_When_Activate_Then_StatusBecomesActive` |
| HH-007 (event) | `Given_DraftHousehold_When_Activate_Then_ExactlyOneHouseholdActivatedEventIsPublished` |
| HH-007 (no BaseCurrency) | `Given_DraftHousehold_WithoutBaseCurrency_When_Activate_Then_DomainExceptionIsThrown` |
| HH-007 (no Owner) | `Given_DraftHousehold_WithoutAnOwner_When_Activate_Then_DomainExceptionIsThrown` |
| Lifecycle (Active) | `Given_ActiveHousehold_When_Activate_Then_DomainExceptionIsThrown` |
| Lifecycle (Archived) | `Given_ArchivedHousehold_When_Activate_Then_DomainExceptionIsThrown` |
| BaseCurrency unchanged | `Given_DraftHousehold_WithBaseCurrency_When_Activate_Then_StatusBecomesActive` |

### AddMember() — implemented

| Command     | Rules Enforced |
| ----------- | -------------- |
| AddMember() | HH-002 (every Member belongs to one Household), HH-005 (Archived cannot accept new members), D3 (exactly one active role), HH-001 (no second Owner) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| HH-002 (belongs) | `Given_ActiveHousehold_When_AddMember_Then_MemberBelongsToTheHousehold`, `Given_DraftHousehold_When_AddMember_Then_MemberBelongsToTheHousehold` |
| HH-002 (unique) | `Given_Household_When_AddingDuplicateMember_Then_DomainExceptionIsThrown` |
| HH-005 (Archived) | `Given_ArchivedHousehold_When_AddMember_Then_DomainExceptionIsThrown` |
| HH-001 (second Owner) | `Given_Household_When_AddingASecondOwner_Then_DomainExceptionIsThrown` |
| D3 (single role) | `Given_ActiveHousehold_When_AddMember_Then_MemberBelongsToTheHousehold` (role asserted) |
| No Domain Event | `Given_Household_When_AddMember_Then_NoDomainEventIsPublished` |

### RemoveMember() — implemented

| Command        | Rules Enforced |
| -------------- | -------------- |
| RemoveMember() | P-02 (removing never removes historical financial data), P-04 (last Owner cannot be removed), preserves HH-001 (exactly one Owner) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| P-02 (only membership) | `Given_HouseholdWithMembers_When_RemoveMember_Then_OnlyMembershipIsAffected` |
| P-04 / HH-001 (last Owner) | `Given_Household_When_RemovingLastOwner_Then_DomainExceptionIsThrown` |
| Unknown member | `Given_Household_When_RemovingUnknownMember_Then_DomainExceptionIsThrown` |
| Removal | `Given_HouseholdWithMembers_When_RemoveMember_Then_MemberIsRemoved` |
| No Domain Event | `Given_Household_When_RemoveMember_Then_NoDomainEventIsPublished` |

### ChangeMemberRole() — implemented

| Command            | Rules Enforced |
| ------------------ | -------------- |
| ChangeMemberRole() | P-05 (last Owner cannot lose the Owner role), D3 (a Member has exactly one active role; the role is replaced, never accumulated), HH-001 (rejects promoting a Member to Owner while an Owner exists) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| P-05 (last Owner) | `Given_HouseholdWithSingleOwner_When_RemovingOwnerRoleFromOwner_Then_DomainExceptionIsThrown` |
| D3 (replaces) | `Given_HouseholdWithTwoOwners_When_DemotingOneOwner_Then_RoleIsReplaced` |
| HH-001 (second Owner) | `Given_HouseholdWithAnOwner_When_PromotingMemberToOwner_Then_DomainExceptionIsThrown` |
| HH-001 (restores Owner) | `Given_HouseholdWithoutOwner_When_PromotingMemberToOwner_Then_ExactlyOneOwnerIsRestored` |
| Unknown member | `Given_Household_When_ChangingRoleOfUnknownMember_Then_DomainExceptionIsThrown` |
| Idempotent same role | `Given_Member_When_ChangeMemberRoleToSameRole_Then_RoleIsUnchanged` |
| No Domain Event | `Given_Household_When_ChangeMemberRole_Then_NoDomainEventIsPublished` |

### Archive() — implemented

| Command   | Rules Enforced |
| --------- | -------------- |
| Archive() | P-01 (only the Owner may archive), HH-005 (Archived cannot accept new members; preserved), lifecycle integrity (Active -> Archived only, no Draft -> Archived) |

| Business Rule | Test Coverage |
| ------------- | ------------- |
| P-01 (Owner archives) | `Given_ActiveHousehold_When_OwnerArchives_Then_StatusBecomesArchived` |
| P-01 (non-Owner rejected) | `Given_ActiveHousehold_When_NonOwnerArchives_Then_DomainExceptionIsThrown`, `Given_ActiveHousehold_When_UnknownMemberArchives_Then_DomainExceptionIsThrown` |
| Lifecycle (event) | `Given_ActiveHousehold_When_OwnerArchives_Then_ExactlyOneHouseholdArchivedEventIsPublished` |
| Lifecycle (Draft rejected) | `Given_DraftHousehold_When_OwnerArchives_Then_DomainExceptionIsThrown` |
| Lifecycle (re-archive rejected) | `Given_ArchivedHousehold_When_OwnerArchives_Then_DomainExceptionIsThrown` |
| P-02 (no data deleted) | `Given_ActiveHousehold_When_OwnerArchives_Then_MembersAndBaseCurrencyAreUnchanged` |