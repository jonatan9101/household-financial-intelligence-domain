# M3 — Register Financial Movement (Application Service)

Approved Technical Design Note (final, with all mandatory adjustments applied):

1. Application must NOT construct the Aggregate's internal Value Objects → `Register(...)` takes primitives and owns VO creation/validation.
2. Repository stays expressed in Domain concepts → `ExistsByEvidenceReferenceAsync(EvidenceReference, ...)` (unchanged from M2).
3. Duplicate detection is an Application-flow guard, performed BEFORE `Register(...)`; the Aggregate receives only the information that defines the FinancialMovement itself.
4. Return `FinancialMovementId` directly (no `RegisterFinancialMovementResult`).
5. Tests must verify repository call order `Exists → Register → AddAsync` and that `Register()` is invoked exactly once.

Files created: `RegisterFinancialMovementCommand`, `RegisterFinancialMovementService` only (no Handlers/MediatR/CQRS/Validators/UnitOfWork/EventBus/Domain Services/Result).

---

## 1. Domain changes

### 1.1 `SharedKernel/DomainErrors.cs` — add constant

```csharp
public static class FinancialMovement
{
    public const string AmountMustBeGreaterThanZero = "Amount must be greater than zero.";
    public const string DuplicateMovement = "A movement with the same evidence reference already exists.";
}
```

### 1.2 `Repositories/IFinancialMovementRepository.cs` — UNCHANGED

```csharp
public interface IFinancialMovementRepository
{
    Task<bool> ExistsByEvidenceReferenceAsync(
        EvidenceReference evidenceReference,
        CancellationToken cancellationToken);

    Task AddAsync(
        FinancialMovement financialMovement,
        CancellationToken cancellationToken);
}
```

The Application may construct an `EvidenceReference` to query the repository — that is using a Domain concept, not constructing Aggregate state.

### 1.3 `Aggregates/FinancialMovement/FinancialMovement.cs` — primitives + owns VOs, NO duplicate flag

`Register(...)` receives only the facts that define the movement. It constructs/validates its own Value Objects and enforces `Amount > 0`. The `ArgumentNullException` guards on VOs are removed (VO constructors throw `DomainException` on null/empty). Private ctor, `DomainEvents`, and `ClearDomainEvents()` stay unchanged.

```csharp
public static FinancialMovement Register(
    HouseholdId householdId,
    FinancialAccountId financialAccountId,
    decimal amount,
    string currency,
    string movementType,
    DateOnly transactionDate,
    string evidenceReference,
    DateTimeOffset occurredAt)
{
    var money = new Money(amount, new Currency(currency));
    var movementTypeValue = new MovementType(movementType);
    var transactionDateValue = new TransactionDate(transactionDate);
    var evidenceReferenceValue = new EvidenceReference(evidenceReference);

    if (money.Amount <= 0)
    {
        throw new DomainException(DomainErrors.FinancialMovement.AmountMustBeGreaterThanZero);
    }

    var movement = new FinancialMovement(
        FinancialMovementId.New(),
        householdId,
        financialAccountId,
        money,
        movementTypeValue,
        transactionDateValue,
        evidenceReferenceValue);

    movement._domainEvents.Add(new FinancialMovementRegistered(
        movement.Id,
        movement.HouseholdId,
        movement.FinancialAccountId,
        movement.Amount.Amount,
        movement.Amount.Currency,
        movement.MovementType,
        occurredAt));

    return movement;
}
```

---

## 2. Application changes

### 2.1 `Application/UseCases/FinancialMovement/RegisterFinancialMovement/RegisterFinancialMovementCommand.cs`

```csharp
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;

namespace HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

public sealed record RegisterFinancialMovementCommand(
    HouseholdId HouseholdId,
    FinancialAccountId FinancialAccountId,
    decimal Amount,
    string Currency,
    string MovementType,
    DateOnly TransactionDate,
    string EvidenceReference,
    DateTimeOffset OccurredAt);
```

### 2.2 `Application/UseCases/FinancialMovement/RegisterFinancialMovement/RegisterFinancialMovementService.cs`

```csharp
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

public sealed class RegisterFinancialMovementService
{
    private readonly IFinancialMovementRepository _repository;

    public RegisterFinancialMovementService(IFinancialMovementRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinancialMovementId> RegisterAsync(
        RegisterFinancialMovementCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var evidenceReference = new EvidenceReference(command.EvidenceReference);

        if (await _repository.ExistsByEvidenceReferenceAsync(evidenceReference, cancellationToken))
        {
            throw new DomainException(DomainErrors.FinancialMovement.DuplicateMovement);
        }

        var movement = FinancialMovement.Register(
            command.HouseholdId,
            command.FinancialAccountId,
            command.Amount,
            command.Currency,
            command.MovementType,
            command.TransactionDate,
            command.EvidenceReference,
            command.OccurredAt);

        await _repository.AddAsync(movement, cancellationToken);

        return movement.Id;
    }
}
```

The service only orchestrates: build the `EvidenceReference` (Domain concept) → existence check (if duplicate, stop the use case BEFORE Aggregate creation with FM-001) → `Register(...)` → `AddAsync` → return id. The Aggregate never receives the repository-query result.

---

## 3. Tests (TDD)

### 3.1 `tests/HouseholdFinancialIntelligence.Tests/RegisterFinancialMovement/RecordingFinancialMovementRepository.cs`

```csharp
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;

namespace HouseholdFinancialIntelligence.Tests.RegisterFinancialMovement;

internal sealed class RecordingFinancialMovementRepository : IFinancialMovementRepository
{
    private readonly HashSet<EvidenceReference> _existingEvidenceReferences = [];

    public List<string> CallLog { get; } = [];

    public FinancialMovement? Added { get; private set; }

    public void Seed(EvidenceReference evidenceReference) => _existingEvidenceReferences.Add(evidenceReference);

    public Task<bool> ExistsByEvidenceReferenceAsync(EvidenceReference evidenceReference, CancellationToken cancellationToken)
    {
        CallLog.Add(nameof(ExistsByEvidenceReferenceAsync));
        return Task.FromResult(_existingEvidenceReferences.Contains(evidenceReference));
    }

    public Task AddAsync(FinancialMovement financialMovement, CancellationToken cancellationToken)
    {
        CallLog.Add(nameof(AddAsync));
        Added = financialMovement;
        return Task.CompletedTask;
    }
}
```

### 3.2 `tests/HouseholdFinancialIntelligence.Tests/RegisterFinancialMovement/RegisterFinancialMovementServiceTests.cs`

Given_When_Then scenarios (FluentAssertions):

1. `Given_ValidCommand_When_Registering_Then_ReturnsANewFinancialMovementId` — result non-empty, not `default`.
2. `Given_ValidCommand_When_Registering_Then_PersistedAggregateReflectsCommandFacts` — `Added` matches household/account/amount/currency/movementType/transactionDate/evidenceReference.
3. `Given_ValidCommand_When_Registering_Then_RepositoryCallsOccurInOrderExistsThenAddAsync` — `CallLog == [Exists, AddAsync]`.
4. `Given_ValidCommand_When_Registering_Then_RegisterIsInvokedExactlyOnce` — `CallLog` has exactly one `AddAsync`; `Added.DomainEvents` has exactly one `FinancialMovementRegistered`; `Added.Id` not default.
5. `Given_ExistingEvidenceReference_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled` — throws `DomainException` with `DuplicateMovement` message; `CallLog == [Exists]`; `Added == null`.
6. `Given_NonPositiveAmount_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled` — amount 0 → `AmountMustBeGreaterThanZero`; `CallLog == [Exists]`.
7. `Given_InvalidEvidenceReference_When_Registering_Then_DomainExceptionIsThrown_AndRepositoryIsNotCalled` — empty/whitespace → VO rule; `CallLog` EMPTY (EvidenceReference throws before the query).
8. `Given_InvalidMovementType_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled` — whitespace → VO rule; `CallLog == [Exists]`.
9. `Given_InvalidCurrency_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled` — `"XX"` → VO rule; `CallLog == [Exists]`.
10. `Given_NullCommand_When_Registering_Then_ArgumentNullExceptionIsThrown` — repository never called (`CallLog` empty).
11. `Given_ValidCommand_When_Registering_Then_ReturnedIdEqualsPersistedAggregateId` — result == `Added.Id`.

### 3.3 Update `tests/.../FinancialMovement/FinancialMovementTests.cs` (M2 domain tests)

- `RegisterValidMovement()` helper → primitives (no exists flag).
- Facts test, event test, non-positive-amount theory → new signature (VOs replaced by primitives).
- DELETE `Given_NullAmount_When_Registering_Then_ArgumentNullExceptionIsThrown` (no longer applicable).
- No duplicate tests in the Domain (duplicate guard is the Application-flow test #5 above).
- Keeps Domain at 100% line + branch.

---

## 4. Quality gates

- `dotnet build` → 0 warnings / 0 errors.
- `dotnet test` → all pass (existing + new).
- Coverage → Domain 100% line + branch; Application service covered by behavior tests.
- Run `architecture-reviewer` then `pr-reviewer`. If either fails: stop, fix, re-run.
- Update `IMPLEMENTATION_PLAN.md` (M3 done).

## 5. Scope exclusions (unchanged)

No EF Core, no API, no DI, no UnitOfWork/commit (transactionality deferred to EF Core milestone), no event publishing (recorded on aggregate only), semantic checks (Household/Account existence/activity) deferred.
