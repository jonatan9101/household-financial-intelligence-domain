# Implementation Plan — HFI MVP (Register Financial Movement)

Approved roadmap. Status: `[ ]` = pending, `[x]` = done.

## Completion Gates (all milestones)

- [ ] Builds successfully
- [ ] All tests pass
- [ ] Leaves the application runnable
- [ ] Demonstrates business value
- [ ] Requires no unfinished infrastructure

---

## M0 — Solution Scaffold

**Status:** done

- **Goal:** Five-project .NET 10 solution with correct dependency rule.
- **Business capability:** Financial Acquisition (foundation).
- **Files expected:** `HouseholdFinancialIntelligence.slnx`, `src/{Api,Application,Domain,Infrastructure}`, `tests/...Tests`, `.gitignore`.
- **Acceptance criteria:** Build passes; Domain references nothing; Tests reference Domain + Application.

Checklist:

- [x] `dotnet build` passes with 0 warnings/errors
- [x] Domain project has no project references
- [x] Tests project references Domain + Application only

---

## M1 — Shared Kernel

**Status:** done

- **Goal:** Small shared types, all tested: `Entity<TId>`, `AggregateRoot<TId>`, `Money`, `Currency`, `DomainException`. No additional abstractions.
- **Business capability:** Financial Acquisition (foundation).
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/Entity.cs`
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/AggregateRoot.cs`
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/Money.cs`
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/Currency.cs`
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/DomainException.cs`
  - Tests: `tests/HouseholdFinancialIntelligence.Tests/SharedKernel/{EntityTests,AggregateRootTests,MoneyTests,CurrencyTests,DomainExceptionTests}.cs`
  - NuGet: `FluentAssertions` added to Tests
- **Tests expected:** identity equality; Money immutability/equality/validation; Currency ISO codes; DomainException for business failures.
- **Acceptance criteria:** `Entity<TId>` value-identity; `AggregateRoot<TId>` extends `Entity<TId>`; Money immutable + rejects invalid; Currency ISO; `DomainException` is the business-rule failure carrier.
- **Dependencies:** M0.

Checklist:

- [x] `Entity<TId>` implemented with identity equality
- [x] `AggregateRoot<TId>` extends `Entity<TId>`
- [x] `Money` immutable, value equality, rejects invalid amounts
- [x] `Currency` supports ISO codes
- [x] `DomainException` implemented
- [x] `FluentAssertions` package added to Tests
- [x] Tests written (`Given_When_Then`)
- [x] Build passes, tests pass

---

## M2 — FinancialMovement Aggregate (minimal)

**Status:** pending

- **Goal:** Minimal aggregate for Register Financial Movement only. Excluded: `MerchantReference`, `MovementStatus`, Categorization, Interpretation, AI concepts.
- **Business capability:** Financial Acquisition → Register Financial Movement.
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/FinancialMovement.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/FinancialMovementId.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/MovementType.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/TransactionDate.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/EvidenceReference.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialMovement/Events/FinancialMovementRegistered.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Repositories/IFinancialMovementRepository.cs` (contract: `ExistsByEvidenceReferenceAsync`, `Add`; NO `Save`)
  - Tests: `FinancialMovementTests`, `MovementTypeTests`, `TransactionDateTests`, `EvidenceReferenceTests`
- **Acceptance criteria:** AC-001 (register creates movement + event); invariants FM-001..004 protected; amount/currency/date immutable; invalid input fails fast (FM-002/003/004); repository contract has no commit verb.
- **Dependencies:** M1.

Checklist:

- [ ] `FinancialMovement` aggregate with `Register(...)` factory
- [ ] `FinancialMovementId`, `MovementType`, `TransactionDate`, `EvidenceReference` value objects (immutable)
- [ ] `FinancialMovementRegistered` domain event
- [ ] `IFinancialMovementRepository` in `Domain/Repositories` (ExistsByEvidenceReferenceAsync + Add, no Save)
- [ ] No MerchantReference / MovementStatus / Categorization / Interpretation
- [ ] Invariants FM-001..004 protected
- [ ] Tests written (register, immutability, FM-002/003/004, event published)
- [ ] Build passes, tests pass

---

## M3 — RegisterFinancialMovement Service

**Status:** pending

- **Goal:** Application orchestration with duplicate detection as a Domain concern.
- **Business capability:** Financial Acquisition → Register Financial Movement.
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Application/UseCases/RegisterFinancialMovement/RegisterFinancialMovementCommand.cs`
  - `src/HouseholdFinancialIntelligence.Application/UseCases/RegisterFinancialMovement/RegisterFinancialMovementService.cs`
  - `src/HouseholdFinancialIntelligence.Application/UseCases/RegisterFinancialMovement/RegisterFinancialMovementResult.cs`
  - `src/HouseholdFinancialIntelligence.Application/Abstractions/IUnitOfWork.cs` (port: `CommitAsync`)
  - Tests: `RegisterFinancialMovementServiceTests` (in-memory fake repository)
- **Flow:** validate → `ExistsByEvidenceReference()` → duplicate returns FM-001 → `Register()` → `Add()` → `CommitAsync()` → return id.
- **Acceptance criteria:** AC-001, AC-002. No `DuplicateDetectionService`, no `CorrelationId`, no idempotency service, no `Save()` in contract.
- **Dependencies:** M2.

Checklist:

- [ ] `RegisterFinancialMovementCommand` input type
- [ ] `RegisterFinancialMovementService` (use-case namespace indicates purpose)
- [ ] `RegisterFinancialMovementResult` (id / business error)
- [ ] `IUnitOfWork` port in `Application/Abstractions`
- [ ] Duplicate detection via `ExistsByEvidenceReference()` → FM-001
- [ ] No DuplicateDetectionService / CorrelationId / idempotency
- [ ] Tests written (happy path + commit called, duplicate no second movement, syntactic rejections)
- [ ] Build passes, tests pass

---

## M4 — Minimal API + In-Memory Repository (First Working MVP)

**Status:** pending

- **Goal:** Capability works end-to-end BEFORE persistence. Runnable locally, no DB, no auth.
- **Business capability:** Register Financial Movement end-to-end.
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/InMemory/InMemoryFinancialMovementRepository.cs`
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/InMemory/InMemoryUnitOfWork.cs` (no-op commit)
  - `src/HouseholdFinancialIntelligence.Api/Program.cs` (DI wiring)
  - `src/HouseholdFinancialIntelligence.Api/Endpoints/FinancialMovementsEndpoints.cs`
  - Syntactic validation + business-error → HTTP mapping (FM-001 → 409, invalid → 400, success → 201)
- **Acceptance criteria:** `POST /api/financial-movements` returns `FinancialMovementId`; duplicate returns `DuplicateMovement`; invalid returns 400. **This is the first usable MVP.**
- **Dependencies:** M3.

Checklist:

- [ ] In-memory repository implementation
- [ ] No-op `IUnitOfWork` implementation
- [ ] DI wiring in `Program.cs`
- [ ] POST endpoint + syntactic validation
- [ ] Error mapping (409 / 400 / 201)
- [ ] Manual smoke: register, duplicate, invalid
- [ ] Build passes

---

## M5 — EF Core + PostgreSQL

**Status:** pending

- **Goal:** Replace in-memory with EF Core persistence. Dev: PostgreSQL via Docker. Prod: Supabase PostgreSQL. Unique constraint on `(HouseholdId, EvidenceReference)` as last line of defense.
- **Business capability:** Financial Acquisition (supporting).
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/HouseholdFinancialDbContext.cs`
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/FinancialMovementRepository.cs`
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/Configurations/FinancialMovementConfiguration.cs`
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/UnitOfWork.cs` (CommitAsync → SaveChangesAsync)
  - `src/HouseholdFinancialIntelligence.Infrastructure/DependencyInjection.cs`
  - EF migration
  - `docker-compose.yml` (dev Postgres)
  - Api `appsettings` dev + Supabase connection strings
  - NuGet: `Microsoft.EntityFrameworkCore`, `Npgsql.EntityFrameworkCore.PostgreSQL`
- **Acceptance criteria:** Migration applies; Save/Load round-trip; duplicate evidence reference violates unique constraint.
- **Dependencies:** M4.

Checklist:

- [ ] EF Core + Npgsql packages added
- [ ] DbContext + FinancialMovement configuration
- [ ] `FinancialMovementRepository` (Add stages entity, no commit)
- [ ] `UnitOfWork` implementing `CommitAsync` → `SaveChangesAsync`
- [ ] DI registration in Infrastructure
- [ ] Migration created and applied
- [ ] Docker Postgres for dev (docker-compose)
- [ ] Supabase connection string for prod in appsettings
- [ ] Unique constraint on (HouseholdId, EvidenceReference)
- [ ] Smoke: save/load round-trip, duplicate constraint violation

---

## M6 — Supabase Authentication

**Status:** pending

- **Goal:** Add auth AFTER the app is usable. Only an authenticated Household member may register; `RequestedBy` from token (SEC-001). MVP remains locally runnable without auth.
- **Business capability:** Financial Acquisition (security).
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Api/Auth/SupabaseAuthExtensions.cs`
  - JWT config in `appsettings`
- **Acceptance criteria:** Unauthorized → SEC-001; member passes; Guest/removed/archived rejected.
- **Dependencies:** M5.

Checklist:

- [ ] Supabase Auth JWT validation
- [ ] Membership check (authenticated user belongs to Household)
- [ ] `RequestedBy` populated from token
- [ ] Manual: authorized → 201, anonymous → 401

---

## M7 — Next.js UI (monorepo)

**Status:** pending

- **Goal:** Minimal screen to register and list movements, calling the API.
- **Business capability:** Register Financial Movement (user-facing).
- **Files expected (single monorepo `/docs /src /tests /web`):**
  - `/web`: Next.js app, Supabase client, register form, movements list, API client
- **Acceptance criteria:** A Household member can register a movement in the UI and see it listed.
- **Dependencies:** M6.

Checklist:

- [ ] Next.js app in `/web`
- [ ] Supabase client configured
- [ ] Register form
- [ ] Movements list
- [ ] API client
- [ ] Manual: register + list through the UI

---

## Key Decisions (locked)

- No CQRS. Terminology: Application Service / Use Case (never "Handler").
- Repository interface lives in `Domain/Repositories`, contract = `ExistsByEvidenceReferenceAsync` + `Add` (no `Save`).
- Commit = minimal `IUnitOfWork.CommitAsync()` (Application port), implemented by Infrastructure (`DbContext.SaveChangesAsync`); no-op for in-memory.
- Duplicate detection: existence check + domain `Register()` + unique constraint (last line of defense). No DuplicateDetectionService / CorrelationId / idempotency.
- Persistence order: In-Memory first, then EF Core. First working MVP before persistence.
- Authentication after the app is usable (M6).
- Dev DB: PostgreSQL via Docker. Prod DB: Supabase PostgreSQL.
- Frontend: monorepo `/docs /src /tests /web`.
- Testing goal: 100% Business Rule Coverage (protect behavior, not line coverage).
