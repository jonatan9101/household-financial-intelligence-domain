# Business Behavior

## Commands

- CreateHousehold
- RenameHousehold
- AddMember
- RemoveMember
- ChangeMemberRole
- ArchiveHousehold

## Domain Events

- HouseholdCreated
- HouseholdRenamed
- MemberAdded
- MemberRemoved
- MemberRoleChanged
- HouseholdArchived

## Policies

- Only the Owner may archive a Household.
- Removing a member never removes historical financial data.
- Ownership transfer must preserve exactly one Owner.

## Lifecycle

Draft -> Active -> Archived
