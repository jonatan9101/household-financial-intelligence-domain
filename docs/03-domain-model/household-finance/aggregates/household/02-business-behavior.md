# Business Behavior

## Commands

- CreateHousehold
- SetBaseCurrency
- Activate
- AddMember
- RemoveMember
- ChangeMemberRole
- ArchiveHousehold

## Published Domain Events

- HouseholdCreated
- HouseholdActivated
- HouseholdArchived

BaseCurrency changes while Draft are internal Aggregate state and publish no event.

## Policies

- P-01 Only the Owner may archive a Household.
- P-02 Removing a member never removes historical financial data.
- P-03 TransferOwnership is deferred until a dedicated business capability is specified.
  TODO — not implemented in the MVP scope.
- P-04 The last Owner cannot be removed.
- P-05 The last Owner cannot lose the Owner role.

Owner is a MemberRole, not a separate flag.

## Business Rules

- A Household starts in Draft.
- BaseCurrency may be assigned or changed only while Draft.
- Activate() requires BaseCurrency to be present (HH-007).
- Once Active, BaseCurrency becomes immutable (HH-003).
- Archived Households cannot change BaseCurrency (HH-003).

## Lifecycle

Draft -> Active -> Archived

- Draft: SetBaseCurrency (assign or change BaseCurrency)
- Draft -> Active: Activate (requires exactly one Owner and a defined BaseCurrency)
- Active -> Archived: Archive (only the Owner may archive)
