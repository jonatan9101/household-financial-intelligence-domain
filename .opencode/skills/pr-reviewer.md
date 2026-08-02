---
name: pr-reviewer
description: Performs a complete architectural and code review before opening a Pull Request. Ensures compliance with DDD, Clean Architecture, Clean Code, SOLID, YAGNI, KISS, DRY, and the project's AI Constitution.
---

# PR Reviewer Skill

## Purpose

Before creating a Pull Request, execute a complete review of the implementation.

The objective is to detect architectural problems, domain violations and unnecessary complexity before human review.

Never approve code simply because it compiles.

Correctness is necessary but not sufficient.

---

# Review Order

Perform reviews in this order.

1. Build

2. Tests

3. Architecture

4. DDD

5. Clean Code

6. SOLID

7. Simplicity

8. Security

9. Performance

10. Documentation

Never skip any section.

---

# STEP 1

## Build Validation

Verify

- Solution builds successfully.
- No compiler warnings.
- No analyzer warnings.
- No nullable warnings.
- No TODO left in production code.
- No commented code.

If any fail

STOP.

---

# STEP 2

## Test Validation

Verify

All tests pass.

No flaky tests.

Meaningful assertions.

Business rules tested.

Target

100% Domain Coverage.

Do NOT measure EF Core coverage.

Do NOT measure ASP.NET coverage.

Measure business behavior.

---

# STEP 3

## Clean Architecture Validation

Verify

Domain references no Infrastructure.

Application references no API.

Infrastructure depends on Domain.

Dependency direction is respected.

No circular dependencies.

No project references violating Clean Architecture.

---

# STEP 4

## DDD Validation

Verify

Aggregates protect invariants.

Entities have identity.

Value Objects are immutable.

Repositories contain no business logic.

Application only orchestrates.

Controllers are thin.

No Anemic Domain Model.

No business logic inside DbContext.

No business logic inside repositories.

No business logic inside controllers.

Business rules exist only inside Domain.

---

# STEP 5

## Vaughn Vernon Validation

Verify

Aggregate boundaries are respected.

No Aggregate modifies another Aggregate.

One transaction per Aggregate.

No leaking invariants.

No invalid Aggregate references.

Cross Aggregate communication uses identities only.

Repositories persist Aggregate Roots only.

---

# STEP 6

## Evans Validation

Verify

Ubiquitous Language respected.

Names match documentation.

No technical names inside Domain.

Business terminology is consistent.

No duplicated business concepts.

---

# STEP 7

## SOLID

Verify

Single Responsibility.

Open Closed.

Liskov.

Interface Segregation.

Dependency Inversion.

Report every violation.

---

# STEP 8

## Clean Code

Verify

Small methods.

Small classes.

Meaningful names.

No magic numbers.

No duplicated logic.

No primitive obsession.

No boolean parameters.

No God Objects.

No hidden side effects.

Low cyclomatic complexity.

---

# STEP 9

## Simplicity

Verify

YAGNI

KISS

DRY

Prefer Composition.

No unnecessary abstraction.

No speculative design.

No premature optimization.

Every abstraction solves a real problem.

---

# STEP 10

## MVP Validation

Verify

No CQRS.

No MediatR.

No Event Bus.

No Kafka.

No RabbitMQ.

No MassTransit.

No Redis.

No Microservices.

No Outbox.

No Event Sourcing.

No Saga.

If any appear

Reject the PR.

---

# STEP 11

## Security

Verify

Input validation.

Authorization respected.

No secrets committed.

No credentials.

No SQL Injection risks.

No insecure serialization.

---

# STEP 12

## EF Core

Verify

Tracking configured correctly.

No N+1 obvious problems.

Correct Aggregate persistence.

No lazy loading.

No business logic inside Entity Configuration.

---

# STEP 13

## Documentation

Verify

If implementation changed business behavior

Update documentation.

Otherwise

Documentation remains unchanged.

Never rewrite documentation unnecessarily.

---

# STEP 14

## Final Review

Before approving ask

Can this be simpler?

Would Vaughn Vernon approve?

Would Eric Evans approve?

Would Robert C. Martin approve?

Can a mid-level .NET developer understand this in 30 minutes?

If any answer is NO

Reject.

Refactor.

Review again.

---

# Output Format

Always produce the following report.

## Build

PASS / FAIL

---

## Tests

PASS / FAIL

Coverage

---

## Clean Architecture

PASS / FAIL

Observations

---

## DDD

PASS / FAIL

Observations

---

## SOLID

PASS / FAIL

Observations

---

## Clean Code

PASS / FAIL

Observations

---

## Simplicity

PASS / FAIL

Observations

---

## Security

PASS / FAIL

Observations

---

## Documentation

PASS / FAIL

Observations

---

## Overall Score

Architecture

0-10

DDD

0-10

Code Quality

0-10

Maintainability

0-10

MVP Simplicity

0-10

Overall

0-10

---

# Approval Rules

Approve only if

- Build passes.
- Tests pass.
- Architecture score >= 9.
- DDD score >= 9.
- Maintainability >= 9.
- No critical issues.
- No unnecessary complexity.

Otherwise

Reject.

Explain why.

Provide concrete refactoring suggestions.

Never approve mediocre code.