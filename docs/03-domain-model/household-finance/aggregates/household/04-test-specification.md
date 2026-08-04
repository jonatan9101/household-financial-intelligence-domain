# Test Specification

## Acceptance Criteria

### AC-001 Create Household
Given valid input
When CreateHousehold executes
Then a Household in Draft with exactly one Owner is created.

### AC-002 Set Base Currency
Given a Draft Household
When SetBaseCurrency executes
Then BaseCurrency is assigned.

### AC-003 Activate Household
Given a Draft Household with exactly one Owner and a defined BaseCurrency
When Activate executes
Then the Household becomes Active.

### AC-004 Activate Fails Without BaseCurrency
Given a Draft Household with exactly one Owner and no BaseCurrency
When Activate executes
Then activation fails (HH-007).

### AC-005 Activate Fails Without an Owner
Given a Draft Household with a defined BaseCurrency and no Owner
When Activate executes
Then activation fails (HH-007).

### AC-006 BaseCurrency Immutable After Activation
Given an Active Household
When SetBaseCurrency executes
Then the change is rejected (HH-003).

### AC-007 Add Member
Given an Active Household
When AddMember executes
Then the member belongs to the Household.

### AC-008 Archive Household
Given an Active Household and the current member is the Owner
When Archive executes
Then the Household becomes Archived and no new members can be added (HH-005).

### AC-009 Archive Only by Owner
Given an Active Household and a Member who is not the Owner
When that Member executes Archive
Then the action is rejected (P-01).

## Domain Tests

- Create household
- Set base currency
- Change base currency while Draft
- Change base currency rejected after activation
- Activate household
- Activate fails without base currency
- Activate fails without an Owner
- Prevent duplicate owner
- Add member
- Remove member
- Change member role
- Archive household
- Archive only by Owner
- Remove the last Owner is rejected (P-04)
- Owner role cannot be removed from the last Owner (P-05)
