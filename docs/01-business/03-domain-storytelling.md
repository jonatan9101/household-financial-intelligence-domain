# Domain Storytelling

## Story 01 - Grocery Purchase

```mermaid
sequenceDiagram
participant User
participant Source
participant Acquisition
participant Interpretation
participant Finance
User->>Source: Purchase
Source-->>Acquisition: Financial Document
Acquisition->>Interpretation: Interpret
Interpretation->>Finance: Register Movement
```

Expected outcome:
- Evidence preserved
- Movement registered
- Household history updated

## Story 02 - Category Correction

Only the interpretation changes.
The financial fact remains immutable.

## Story 03 - Duplicate Notification

Duplicate documents must never create duplicate financial movements.
