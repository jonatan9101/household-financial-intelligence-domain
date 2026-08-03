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

    private static RegisterFinancialMovementService CreateService(RecordingFinancialMovementRepository repository) =>
        new(repository);

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_ReturnsANewFinancialMovementId()
    {
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var result = await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        result.Value.Should().NotBe(Guid.Empty);
        result.Should().NotBe(default(FinancialMovementId));
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_PersistedAggregateReflectsCommandFacts()
    {
        var command = ValidCommand();
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

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
    public async Task Given_ValidCommand_When_Registering_Then_RepositoryCallsOccurInOrderExistsThenAddAsync()
    {
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync), nameof(RecordingFinancialMovementRepository.AddAsync));
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_RegisterIsInvokedExactlyOnce()
    {
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

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
        var repository = new RecordingFinancialMovementRepository();
        repository.Seed(new EvidenceReference(command.EvidenceReference));
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.FinancialMovement.DuplicateMovement);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
    }

    [Fact]
    public async Task Given_NonPositiveAmount_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { Amount = 0 };
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.FinancialMovement.AmountMustBeGreaterThanZero);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
    }

    [Fact]
    public async Task Given_InvalidEvidenceReference_When_Registering_Then_DomainExceptionIsThrown_AndRepositoryIsNotCalled()
    {
        var command = ValidCommand() with { EvidenceReference = "   " };
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.EvidenceReference.Required);
        repository.CallLog.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_InvalidMovementType_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { MovementType = "   " };
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.MovementType.CannotBeNullOrEmpty);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
    }

    [Fact]
    public async Task Given_InvalidCurrency_When_Registering_Then_DomainExceptionIsThrown_AndAddAsyncIsNotCalled()
    {
        var command = ValidCommand() with { Currency = "XX" };
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainException>()
            .WithMessage(DomainErrors.Currency.InvalidIso4217Code);
        repository.CallLog.Should().Equal(nameof(RecordingFinancialMovementRepository.ExistsByEvidenceReferenceAsync));
        repository.Added.Should().BeNull();
    }

    [Fact]
    public async Task Given_NullCommand_When_Registering_Then_ArgumentNullExceptionIsThrown()
    {
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var action = () => service.RegisterAsync(null!, CancellationToken.None);

        await action.Should().ThrowAsync<ArgumentNullException>();
        repository.CallLog.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_ValidCommand_When_Registering_Then_ReturnedIdEqualsPersistedAggregateId()
    {
        var repository = new RecordingFinancialMovementRepository();
        var service = CreateService(repository);

        var result = await service.RegisterAsync(ValidCommand(), CancellationToken.None);

        repository.Added.Should().NotBeNull();
        result.Should().Be(repository.Added!.Id);
    }
}
