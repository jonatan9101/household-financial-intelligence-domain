# AGENTS.md

# Household Financial Intelligence (HFI)

## AI Development Constitution

This document defines the mandatory engineering rules for every AI agent
(OpenCode, Claude Code, Cursor, GitHub Copilot, etc.) working on this repository.

These rules are mandatory.

If multiple instructions conflict, follow the highest priority rule.

---

# 1. Mission

Your mission is NOT generating code.

Your mission is building a maintainable Household Financial Intelligence system.

Always protect:

- Domain Model
- Simplicity
- Readability
- Maintainability

Never optimize for hypothetical future requirements.

---

# 2. Project Scope

This is NOT a SaaS.

This application is initially intended for a single household.

Expected users

5–10

Expected traffic

Very low.

Expected financial movements

20–50 per day.

Infrastructure budget

USD $0.

Every technical decision must be justified by today's needs.

---

# 3. Source of Truth

The documentation is the project's Constitution.

Read documentation in this order.

1.

docs/README_FOR_AI.md

2.

docs/adr/

3.

docs/03-domain-model/

4.

docs/02-strategic-design/

5.

docs/01-business/

6.

docs/04-architecture/

Never invent business rules.

If documentation is ambiguous

STOP.

Ask for clarification.

---

# 4. Development Strategy

Implement ONE capability at a time.

Never implement multiple business capabilities simultaneously.

Complete the current capability before starting another.

Current priority

Financial Acquisition

↓

Register Financial Movement

Nothing else.

---

# 5. Architecture

Use Simplified Clean Architecture.

Projects

HouseholdFinancialIntelligence.Api

HouseholdFinancialIntelligence.Application

HouseholdFinancialIntelligence.Domain

HouseholdFinancialIntelligence.Infrastructure

HouseholdFinancialIntelligence.Tests

Dependency Rule

Api

↓

Application

↓

Domain

↑

Infrastructure

The Domain project must never depend on Infrastructure.

---

# 6. Domain Driven Design

Follow

Eric Evans

Vaughn Vernon

Business rules belong ONLY inside the Domain.

Aggregates protect invariants.

Repositories persist Aggregates.

Repositories never contain business logic.

Application orchestrates.

Infrastructure implements.

Controllers are thin.

Avoid Anemic Domain Models.

Favor Rich Domain Models.

Value Objects must be immutable.

Entities protect consistency.

---

# 7. Engineering Principles

Always follow

SOLID

KISS

DRY

YAGNI

Tell Don't Ask

Fail Fast

Composition over Inheritance

Law of Demeter

Design by Contract

Encapsulation

Prefer explicit code over magic.

Never introduce abstractions without a real use case.

Every abstraction must solve an existing problem.

---

# 8. Clean Code

Write code that a mid-level .NET developer can understand.

Requirements

Small methods.

Expressive names.

Single Responsibility.

No primitive obsession.

No boolean parameters.

No God Classes.

No hidden side effects.

Avoid comments explaining bad code.

Code should explain itself.

---

# 9. MVP Rules

DO NOT implement

CQRS

MediatR

Event Bus

Kafka

RabbitMQ

MassTransit

Outbox

Read Models

Background Workers

Distributed Transactions

Microservices

Redis

Caching

Event Sourcing

Saga

YAGNI applies.

---

# 10. Technology Stack

Frontend

- Next.js

Backend

- .NET 10
- ASP.NET Core Minimal API

Persistence

- PostgreSQL (Supabase)

Authentication

- Supabase Auth

Storage

- Supabase Storage

ORM

- Entity Framework Core

Testing

- xUnit
- FluentAssertions

CI

- GitHub Actions

Do not introduce new technologies without explicit approval.

---

# 11. Testing Strategy

Practice Test-Driven Development whenever practical.

Every business rule must have tests.

Target

100% Domain Coverage.

Test

Value Objects

Aggregates

Factories

Policies

Application Services

Do NOT test

Entity Framework

ASP.NET

Dependency Injection

Configuration

Prefer behavior over implementation.

Naming convention

Given_When_Then

---

# 12. Simplicity Rules

Before introducing code ask

Can this be simpler?

Can this be deleted?

Is this solving today's problem?

Does this violate YAGNI?

If yes

Simplify.

---

# 13. Refactoring Rules

Refactor only when

Readability improves.

Complexity decreases.

Duplication decreases.

Never refactor for personal preference.

Never reorganize the project structure without approval.

---

# 14. Working Agreement

Implement one step at a time.

Never generate an entire application in one iteration.

After every completed step

Explain

- Design decisions
- Files created
- Tests created
- Trade-offs
- Next step

Stop.

Wait for approval.

---

# 15. Decision Checklist

Before every commit verify

✓ Does this respect Evans?

✓ Does this respect Vernon?

✓ Does this respect Clean Architecture?

✓ Does this respect SOLID?

✓ Does this respect DRY?

✓ Does this respect KISS?

✓ Does this respect YAGNI?

✓ Is the Domain still isolated?

✓ Is this the simplest possible solution?

If any answer is NO

Refactor before continuing.

---

# 16. Success Criteria

Success is NOT measured by

- Number of classes
- Number of abstractions
- Number of design patterns

Success is measured by

- Business value delivered
- Simplicity
- Readability
- Correct Domain Model
- Low maintenance cost
- Zero-cost infrastructure
- Passing tests

Always choose the simplest solution that correctly models the domain.

# 17. Stop Rules

The AI agent MUST stop immediately when:

- A business rule is ambiguous.
- Documentation conflicts with implementation.
- A new architectural pattern seems necessary.
- A new dependency is required.
- A new project is proposed.
- A change affects more than one Aggregate.

Instead of guessing:

1. Explain the issue.
2. Present the available options.
3. Recommend the simplest solution.
4. Wait for human approval.

An Application Service may orchestrate the execution of business rules,
but it must never decide them.

If moving a line of code from the Application layer to the Aggregate changes business behavior,
that line belongs in the Domain.

Application Services must never instantiate Value Objects
that belong to an Aggregate unless explicitly documented.

Whenever possible, Aggregates construct and own their internal model.