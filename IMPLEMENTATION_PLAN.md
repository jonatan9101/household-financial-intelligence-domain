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

**Status:** done

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

- [x] `FinancialMovement` aggregate with `Register(...)` factory
- [x] `FinancialMovementId`, `MovementType`, `TransactionDate`, `EvidenceReference` value objects (immutable)
- [x] `FinancialMovementRegistered` domain event
- [x] `IFinancialMovementRepository` in `Domain/Repositories` (ExistsByEvidenceReferenceAsync + Add, no Save)
- [x] No MerchantReference / MovementStatus / Categorization / Interpretation
- [x] Invariants FM-001..004 protected
- [x] Tests written (register, immutability, FM-002/003/004, event published)
- [x] Build passes, tests pass

---

## M3 — RegisterFinancialMovement Service

**Status:** done

- **Goal:** Application orchestration for Register Financial Movement. The Application never makes business decisions: `FinancialMovement.Register(...)` receives only the facts that define the movement and constructs/validates its own Value Objects.
- **Business capability:** Financial Acquisition → Register Financial Movement.
- **Files expected:**
  - `src/HouseholdFinancialIntelligence.Application/UseCases/FinancialMovement/RegisterFinancialMovement/RegisterFinancialMovementCommand.cs`
  - `src/HouseholdFinancialIntelligence.Application/UseCases/FinancialMovement/RegisterFinancialMovement/RegisterFinancialMovementService.cs`
  - Tests: `RegisterFinancialMovementServiceTests` (+ `RecordingFinancialMovementRepository` fake)
- **Flow:** build `EvidenceReference` → `ExistsByEvidenceReferenceAsync()` → duplicate throws FM-001 (before Aggregate creation) → `Register()` → `AddAsync()` → return `FinancialMovementId`.
- **Acceptance criteria:** AC-001, AC-002. No Handlers / MediatR / CQRS / Validators / UnitOfWork / EventBus / Domain Services / Result class. `IFinancialMovementRepository` contract unchanged (Domain concepts: `EvidenceReference`). No `Save()`.
- **Dependencies:** M2.

Checklist:

- [x] `RegisterFinancialMovementCommand` input type (IDs + primitives + OccurredAt)
- [x] `RegisterFinancialMovementService` (orchestration only; returns `FinancialMovementId` directly)
- [x] `FinancialMovement.Register(...)` receives only defining facts; owns VO creation/validation; enforces `Amount > 0`
- [x] Duplicate detection in the Application flow via `ExistsByEvidenceReferenceAsync()` → FM-001, stops before `Register()`
- [x] No DuplicateDetectionService / CorrelationId / idempotency / UnitOfWork
- [x] Tests written (call order `Exists → AddAsync`, `Register()` invoked once, duplicate, syntactic rejections, returned id)
- [x] Build passes (0 warnings/errors); 76 tests pass; Domain + Application 100% line/branch coverage

---

## M4 — EF Core + PostgreSQL Persistence (no API)

**Status:** done

- **Goal:** Persistence ONLY for the existing `FinancialMovement` aggregate. No API, no endpoints, no DI, no middleware, no `IUnitOfWork`, no `SaveChangesAsync` inside the repository. The transaction is completed explicitly (smoke) and later by the Application/API layer (M5).
- **Business capability:** Financial Acquisition (supporting).
- **Files created:**
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/HouseholdFinancialDbContext.cs` (`DbSet<FinancialMovement>`, applies configuration)
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/FinancialMovementRepository.cs` (stages only — `AnyAsync` for `ExistsByEvidenceReferenceAsync`, `AddAsync` WITHOUT commit)
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/Configurations/FinancialMovementConfiguration.cs`
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/DesignTimeDbContextFactory.cs` (dev connection string for `dotnet ef`)
  - EF migration `InitialCreate` + model snapshot
  - `docker-compose.yml` (dev Postgres 16)
  - `tools/PersistenceSmoke` (versioned round-trip smoke, NOT in slnx)
  - NuGet: `Microsoft.EntityFrameworkCore` 10.0.10, `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3
- **Mapping decisions:** `Money` = EF owned type → two real columns `Amount` (numeric(18,2)) + `CurrencyCode` (varchar(3)); all other VOs and IDs via value converters; unique index `UX_FinancialMovement_Household_EvidenceReference` on `(HouseholdId, EvidenceReference)` as last line of defense.
- **Proven EF Core limitation (root cause):** owned/complex navigations cannot be bound to constructor parameters (`Cannot bind 'amount'...`), and `FinancialMovement` had no write path for `Amount`. Minimal, behavior-neutral Domain adaptation approved and applied: `Amount` moved out of the private constructor into a private backing field `_amount` (get-only property preserved → aggregate stays immutable, `CanWrite == false`), assigned in the `Register` factory; EF injects via the field (`HasField("_amount")`, `FieldDuringConstruction`). `Money`, `Entity<TId>`, `AggregateRoot<TId>`, and the public API are untouched. Infrastructure adapts to the Domain.
- **Acceptance criteria:** Migration applies; save/load round-trip of every VO/ID; duplicate evidence reference violates the unique constraint; `SaveChangesAsync` runs explicitly outside the repository; Domain/Application/Tests unchanged except the proven minimal adaptation.
- **Dependencies:** M3.

Checklist:

- [x] EF Core + Npgsql packages added
- [x] DbContext + `FinancialMovementConfiguration` (OwnsOne Money → 2 columns, converters, unique index)
- [x] `FinancialMovementRepository` (Add stages entity, no commit)
- [x] Design-time factory for `dotnet ef`
- [x] Migration created and applied (Docker Postgres)
- [x] Docker Postgres for dev (docker-compose)
- [x] `tools/PersistenceSmoke` round-trip: migrate, save, load, duplicate → `DbUpdateException`, SMOKE PASSED
- [x] Build 0 warnings/errors; existing 76 tests pass unchanged

---

## M5 — Minimal API + HTTP Mapping + DI

**Status:** done

- **Goal:** Expose Register Financial Movement over HTTP. Transaction completes inside the Application use case via a minimal `ISaveChanges` port (never inside the repository, never in the endpoint). Exception middleware maps business errors to HTTP.
- **Business capability:** Register Financial Movement end-to-end.
- **Files created:**
  - `src/HouseholdFinancialIntelligence.Api/Program.cs` (DI wiring; no migrations at startup)
  - `src/HouseholdFinancialIntelligence.Api/Endpoints/FinancialMovementsEndpoints.cs` (request DTO + `POST /api/financial-movements`)
  - `src/HouseholdFinancialIntelligence.Api/Middleware/DomainExceptionMiddleware.cs` (ProblemDetails)
  - `src/HouseholdFinancialIntelligence.Application/Persistence/ISaveChanges.cs` (single-method Application port)
  - `src/HouseholdFinancialIntelligence.Infrastructure/Persistence/SaveChanges.cs` (delegates to `DbContext.SaveChangesAsync`)
  - `src/HouseholdFinancialIntelligence.Infrastructure/DependencyInjection.cs` (DbContext, repository, `ISaveChanges`)
  - `src/HouseholdFinancialIntelligence.Domain/SharedKernel/DomainErrorCode.cs` (strongly typed error code, no primitive obsession)
  - `DomainException.ErrorCode` + `DomainErrors.FinancialMovement.DuplicateMovementCode` (`FM-001`)
  - Api `appsettings.json` / `appsettings.Development.json` connection strings
- **Mapping:** `DomainErrorCode FM-001` → 409; other `DomainException` → 400; invalid/malformed body → 400; `DbUpdateException` (unique-index race) → 409; unhandled → 500. Success → 201 `{ id }` + Location.
- **Migrations are NOT auto-applied at startup** — the application never mutates the database. Schema is applied explicitly.
- **Runbook (dev):**
  1. `docker compose up -d` (Postgres 16, container `hfi-postgres`)
  2. Apply migrations explicitly: `dotnet ef database update --project src/HouseholdFinancialIntelligence.Infrastructure --startup-project tools/PersistenceSmoke`
  3. Run: `dotnet run --project src/HouseholdFinancialIntelligence.Api` (Development reads `appsettings.Development.json`; prod must set `ConnectionStrings__Default`)
  4. Smoke: `curl -X POST localhost:5114/api/financial-movements -H "Content-Type: application/json" -d @body.json` → 201; repeat → 409 `FM-001`; invalid input → 400.
- **Acceptance criteria:** `POST /api/financial-movements` returns `FinancialMovementId`; duplicate → 409; invalid → 400. **First usable MVP.**
- **Dependencies:** M4.

Checklist:

- [x] DI registration (DbContext, repository, `ISaveChanges`) in Infrastructure
- [x] `ISaveChanges` Application port — transaction completes in the use case; repository and endpoint stay clean
- [x] Exception middleware → ProblemDetails (409 / 400 / 500)
- [x] POST endpoint + request DTO + endpoint-only syntactic validation
- [x] Strongly typed `DomainErrorCode` (FM-001) — no primitive obsession
- [x] No migrations at startup; migration execution stays explicit
- [x] Manual smoke: register → 201, duplicate → 409, invalid/malformed → 400
- [x] Build 0 warnings/errors; 91 tests pass; Domain + Application 100% line/branch coverage

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

## M8 — FinancialAccount Aggregate (Domain)

**Status:** completed

- **Goal:** Implement the FinancialAccount aggregate end-to-end (4 commands) following the one-command-per-iteration flow, keeping the Domain isolated.
- **Business capability:** Financial Acquisition → maintain Financial Account metadata and lifecycle.
- **Commands:** Register, Rename, Close, Reopen.
- **Files created:**
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialAccount/FinancialAccount.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialAccount/{FinancialAccountId,AccountName,AccountIdentifier,AccountType,InstitutionName,AccountStatus}.cs`
  - `src/HouseholdFinancialIntelligence.Domain/Aggregates/FinancialAccount/Events/{FinancialAccountRegistered,FinancialAccountRenamed,FinancialAccountClosed,FinancialAccountReopened}.cs`
  - `docs/03-domain-model/household-finance/aggregates/financial-account/{01..06}.md`
- **Business rules:** FA-001 . FA-010 covered (matrix Implemented; FA-004/FA-010 cross-aggregate delegated to FinancialMovement, FA-009 to persistence).
- **Tests:** 29 domain tests (Given_When_Then); Domain coverage 100% line + 100% branch (39 classes).
- **Reviewers:** business, aggregate, architecture, code, pr - all APPROVED on each command and at closure.
- **Acceptance criteria:** All four commands protected by rules; lifecycle Active <-> Closed explicit; aggregate does not touch FinancialMovement/balances/other Aggregates.

Checklist:

- [x] RegisterFinancialAccount implemented + tested
- [x] RenameFinancialAccount implemented + tested
- [x] CloseFinancialAccount implemented + tested
- [x] ReopenFinancialAccount implemented + tested
- [x] Business Rule Matrix: no Pending entries
- [x] Command Traceability at 100%
- [x] 100% line and branch coverage (Domain)
- [x] CHANGELOG updated
- [x] Aggregate marked Completed

---

## Documentation Backlog

- [ ] **TODO** — Define the canonical MovementType taxonomy. `MovementType` is currently an unconstrained string Value Object (not null / not empty / trimmed). The FM-003 `UnsupportedMovementType` business rule must NOT be enforced until an authoritative taxonomy exists (the Financial Movement examples in `docs/01-business/01-ubiquitous-language.md` are illustrative, not normative). Once defined, `MovementType` can evolve into a constrained Value Object or enum without changing the aggregate's responsibilities.

## Key Decisions (locked)

- No CQRS. Terminology: Application Service / Use Case (never "Handler").
- Repository interface lives in `Domain/Repositories`, contract = `ExistsByEvidenceReferenceAsync` + `AddAsync` (no `Save`).
- Repository stages entities; the transaction is completed inside the Application use case via the minimal `ISaveChanges` port (single `SaveChangesAsync`), implemented by Infrastructure. No `IUnitOfWork`, no commit inside the repository, no commit in the endpoint.
- Duplicate detection: existence check + domain `Register()` + unique constraint (last line of defense). No DuplicateDetectionService / CorrelationId / idempotency.
- Persistence order: EF Core + PostgreSQL persistence first (M4), Minimal API after (M5). No in-memory repository.
- Authentication after the app is usable (M6).
- Dev DB: PostgreSQL via Docker. Prod DB: Supabase PostgreSQL.
- Frontend: monorepo `/docs /src /tests /web`.
- Testing goal: 100% Business Rule Coverage (protect behavior, not line coverage).
