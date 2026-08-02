# Test Specification

## Acceptance Criteria

### AC-001 Create Household
Given valid input
When CreateHousehold executes
Then a Household with one Owner is created.

### AC-002 Add Member
Given an active Household
When AddMember executes
Then the member belongs to the Household.

### AC-003 Archive Household
Given an active Household
When ArchiveHousehold executes
Then no new members can be added.

## Domain Tests

- Create household
- Prevent duplicate owner
- Add member
- Remove member
- Archive household
