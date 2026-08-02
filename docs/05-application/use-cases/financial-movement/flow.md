# Main Flow

```mermaid
sequenceDiagram

actor User

participant API

participant Handler

participant Repository

participant Aggregate

participant EventBus

User->>API:RegisterFinancialMovement

API->>Handler:Command

Handler->>Repository:Load Household

Handler->>Repository:Load Account

Handler->>Aggregate:Register()

Aggregate-->>Handler:FinancialMovementCreated

Handler->>Repository:Save()

Handler->>EventBus:Publish()

Handler-->>API:FinancialMovementId
```

---

## Alternative Flow 01

Duplicate movement.

Result

DuplicateMovementDetected

---

Alternative Flow 02

Household inactive.

---

Alternative Flow 03

Financial Account not found.

---

Alternative Flow 04

Unauthorized user.

---

Alternative Flow 05

Unsupported currency.