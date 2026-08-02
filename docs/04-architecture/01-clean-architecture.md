# Clean Architecture

## Layers

```text
Presentation
    ↓
Application
    ↓
Domain
    ↓
Infrastructure
```

## Responsibilities

### Presentation
- HTTP API
- Authentication
- Validation
- Request mapping

### Application
- Use Cases
- Command Handlers
- Query Handlers
- Transactions

### Domain
- Aggregates
- Value Objects
- Domain Events
- Policies

### Infrastructure
- Persistence
- Messaging
- Storage
- External integrations

## Dependency Rule

Source code dependencies always point toward the Domain layer.
