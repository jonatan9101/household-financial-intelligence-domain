# CQRS Implementation

## Command Flow

API
→ Command
→ Command Handler
→ Aggregate
→ Repository
→ Domain Events

## Query Flow

API
→ Query
→ Query Handler
→ Read Model

## Principles

- Commands return identifiers or acknowledgements.
- Queries return DTOs.
- Read models are eventually consistent.
