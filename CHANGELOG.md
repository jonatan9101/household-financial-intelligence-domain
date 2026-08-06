# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/) and this project adheres to [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- **FinancialAccount aggregate (complete):** `Register`, `Rename`, `Close`, `Reopen` commands; FA-001..FA-010 rules; `FinancialAccountRegistered/Renamed/Closed/Reopened` domain events; Value Objects (`AccountName`, `AccountIdentifier`, `AccountType`, `InstitutionName`, `AccountStatus`). 29 domain tests, Domain 100% line + branch coverage.

## [0.1.0] - 2026-08-03

Foundation milestone — end-to-end MVP for **Register Financial Movement**.

- **M1 — Shared Kernel:** `Entity<TId>`, `AggregateRoot<TId>`, `Money`, `Currency`, `DomainException`.
- **M2 — FinancialMovement aggregate:** immutable aggregate, value objects, `FinancialMovementRegistered` event, repository contract.
- **M3 — RegisterFinancialMovement service:** Application orchestration, duplicate detection, unit-tested use case.
- **M4 — Persistence:** EF Core + PostgreSQL, `Money` as Owned Type (two columns), unique index, smoke round-trip, Docker dev database.
- **M5 — Minimal API:** `POST /api/financial-movements`, exception middleware → ProblemDetails, strong `DomainErrorCode`, `ISaveChanges` port, Dependency Injection, first usable MVP.

[0.1.0]: https://github.com/jonatan9101/household-financial-intelligence-domain/releases/tag/v0.1.0-alpha