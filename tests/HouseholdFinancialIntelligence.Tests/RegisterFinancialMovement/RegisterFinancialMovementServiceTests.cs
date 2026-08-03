using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.Events;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;
using HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

namespace HouseholdFinancialIntelligence.Tests.RegisterFinancialMovement;

public class RegisterFinancialMovementServiceTests
{
    private static readonly DateTimeOffset OccurredAt = new(2026, 7, 1, 12, 0, 0, TimeSpan.Zero);

    private static RegisterFinancialMovementCommand ValidCommand() =>
        new(
            new HouseholdId(Guid.NewGuid()),
            new FinancialAccountId(Guid.NewGuid()),
            150.00m,
            "USD",
            "Purchase",
            new DateOnly(2026, 7, 1),
            "receipt-2026-07-001",
            OccurredAt);

    private static (
        RegisterFinancialMovementService Service,
        RecordingFinancialMovementRepository Repository,
        RecordingSaveChanges SaveChanges) CreateService()
    {
        var callLog = new List<string>();
        var repository = new RecordingFinancialMovementRepository(callLog);
        var saveChanges = new RecordingSaveChanges(callLog);
        return (new RegisterFinancialMovementService(repository, saveChanges), repository, saveChanges);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_ReturnsANewFinancialMovementId()
    {
        var (service, repository, _) = CreateService();

        var result = await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        result.Value.Should().NotBe(Guid.Empty);
        result.Should().NotBe(default(FinancialMovementId));
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_PersistedAggregateReflectsCommandFacts()
    {
        var command = ValidCommand();
        var (service, repository, _) = CreateService();

        await service.RegisterAsync(command, CancellationToken.None);

        repository.Added.Should().NotBeNull();
        var added = repository.Added!;
        added.HouseholdId.Should().Be(command.HouseholdId);
        added.FinancialAccountId.Should().Be(command.FinancialAccountId);
        added.Amount.Should().Be(new Money(command.Amount, new Currency(command.Currency)));
        added.MovementType.Should().Be(new MovementType(command.MovementType));
        added.TransactionDate.Should().Be(new TransactionDate(command.TransactionDate));
        added.EvidenceReference.Should().Be(new EvidenceReference(command.EvidenceReference));
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_RepositoryCallsOccurInOrderExistsAddThenSaveChanges()
    {
        var (service, repository, _) = CreateService();

        await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        repository.CallLog.Should().Equal(
            nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync),
            nameof(RecordingFinancialMovementRepository.AddAsync),
            nameof(RecordingSaveChanges.SaveChangesAsync));
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_SaveChangesIsCalledExactlyOnce()
    {
        var (service, _, saveChanges) = CreateService();

        await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        saveChanges.CallLog.Count(entry => entry == nameof(RecordingSaveChanges.SaveChangesAsync)).Should().Be(1);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_RegisterIsInvokedExactlyOnce()
    {
        var (service, repository, _) = CreateService();

        await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        repository.CallLog.Count(entry => entry == nameof(RecordingFinancialMovementRepository.AddAsync)).Should().Be(1);
        repository.Added.Should().NotBeNull();
        var added = repository.Added!;
        added.Id.Value.Should().NotBe(Guid.Empty);
        added.DomainEvents.Should().HaveCount(1);
        added.DomainEvents.Single().Should().BeOfType<FinancialMovementRegistered>();
    }

    [Fact]
    public async Task Given_ExistingEvidenceReference_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand();
        var (service, repository, saveChanges) = CreateService();
        repository.Seed(new EvidenceReference(command.EvidenceReference));

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.FinancialMovement.DuplicateMovement);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
        saveChanges.CallLog.Should().NotContain(nameof(RecordingSaveChanges.SaveChangesAsync));
    }

    [Fact]
    public async Task Given_ExistingEvidenceReference_When_Registering_Then_DomainExceptionCarriesDuplicateMovementCode()
    {
        var command = ValidCommand();
        var (service, repository, _) = CreateService();
        repository.Seed(new EvidenceReference(command.EvidenceReference));

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .Where(exception => exception.ErrorCode == DomainErrors.FinancialMovement.DuplicateMovementCode);
    }

    [Fact]
    public async Task Given_NonPositiveAmount_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { Amount = 0 };
        var (service, repository, saveChanges) = CreateService();

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.FinancialMovement.AmountMustBeGreaterThanZero);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
        saveChanges.CallLog.Should().NotContain(nameof(RecordingSaveChanges.SaveChangesAsync));
    }

    [Fact]
    public async Task Given_InvalidEvidenceReference_When_Registering_Then_DomainExceptionIsThrown_AndRepositoryIsNotCalled()
    {
        var command = ValidCommand() with { EvidenceReference = "   " };
        var (service, repository, saveChanges) = CreateService();

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.EvidenceReference.Required);
        repository.CallLog.Should().BeEmpty();
        saveChanges.CallLog.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_InvalidMovementType_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { MovementType = "   " };
        var (service, repository, saveChanges) = CreateService();

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.MovementType.CannotBeNullOrEmpty);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
        saveChanges.CallLog.Should().NotContain(nameof(RecordingSaveChanges.SaveChangesAsync));
    }

    [Fact]
    public async Task Given_InvalidCurrency_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { Currency = "XX" };
        var (service, repository, saveChanges) = CreateService();

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.Currency.InvalidIso4217Code);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
        saveChanges.CallLog.Should().NotContain(nameof(RecordingSaveChanges.SaveChangesAsync));
    }

    [Fact]
    public async Task Given_NullCommand_When_Registering_Then_ArgumentNullExceptionIsThrown()
    {
        var (service, repository, saveChanges) = CreateService();

        var action = () => service.RegisterAsync(null!, CancellationToken.None);

        await action.Should().ThrowAsync<ArgumentNullException>();
        repository.CallLog.Should().BeEmpty();
        saveChanges.CallLog.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_ReturnedIdEqualsPersistedAggregateId()
    {
        var (service, repository, _) = CreateService();

        var result = await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        repository.Added.Should().NotBeNull();
        result.Should().Be(repository.Added!.Id);
    }
}
