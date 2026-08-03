using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.Events;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class FinancialMovementTests
{
    private static readonly Currency Usd = new("USD");

    private static FinancialMovement RegisterValidMovement() =>
        FinancialMovement.Register(
            new HouseholdId(Guid.NewGuid()),
            new FinancialAccountId(Guid.NewGuid()),
            new Money(150.00m, Usd),
            new MovementType("Purchase"),
            new TransactionDate(new DateOnly(2026, 7, 1)),
            new EvidenceReference("receipt-2026-07-001"));

    [Fact]
    public void Given_ValidFacts_When_Registering_Then_AggregateReflectsThoseFacts()
    {
        var householdId = new HouseholdId(Guid.NewGuid());
        var financialAccountId = new FinancialAccountId(Guid.NewGuid());
        var amount = new Money(150.00m, Usd);
        var movementType = new MovementType("Purchase");
        var transactionDate = new TransactionDate(new DateOnly(2026, 7, 1));
        var evidenceReference = new EvidenceReference("receipt-2026-07-001");

        var movement = FinancialMovement.Register(
            householdId,
            financialAccountId,
            amount,
            movementType,
            transactionDate,
            evidenceReference);

        movement.Id.Value.Should().NotBe(Guid.Empty);
        movement.HouseholdId.Should().Be(householdId);
        movement.FinancialAccountId.Should().Be(financialAccountId);
        movement.Amount.Should().Be(amount);
        movement.MovementType.Should().Be(movementType);
        movement.TransactionDate.Should().Be(transactionDate);
        movement.EvidenceReference.Should().Be(evidenceReference);
    }

    [Fact]
    public void Given_ValidRegistration_When_Registering_Then_ExactlyOneFinancialMovementRegisteredEventIsPublished()
    {
        var movement = RegisterValidMovement();

        movement.DomainEvents.Should().HaveCount(1);
        var registered = movement.DomainEvents.Single();
        registered.FinancialMovementId.Should().Be(movement.Id);
        registered.HouseholdId.Should().Be(movement.HouseholdId);
        registered.FinancialAccountId.Should().Be(movement.FinancialAccountId);
        registered.Amount.Should().Be(movement.Amount.Amount);
        registered.Currency.Should().Be(movement.Amount.Currency);
        registered.MovementType.Should().Be(movement.MovementType);
        registered.OccurredAt.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public void Given_NonPositiveAmount_When_Registering_Then_DomainExceptionIsThrown(decimal amount)
    {
        var action = () => FinancialMovement.Register(
            new HouseholdId(Guid.NewGuid()),
            new FinancialAccountId(Guid.NewGuid()),
            new Money(amount, Usd),
            new MovementType("Purchase"),
            new TransactionDate(new DateOnly(2026, 7, 1)),
            new EvidenceReference("receipt-2026-07-001"));

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_NullAmount_When_Registering_Then_ArgumentNullExceptionIsThrown()
    {
        var action = () => FinancialMovement.Register(
            new HouseholdId(Guid.NewGuid()),
            new FinancialAccountId(Guid.NewGuid()),
            null!,
            new MovementType("Purchase"),
            new TransactionDate(new DateOnly(2026, 7, 1)),
            new EvidenceReference("receipt-2026-07-001"));

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_AMovement_When_InspectingFacts_Then_TheyAreImmutable()
    {
        var movement = RegisterValidMovement();

        typeof(FinancialMovement).GetProperty(nameof(movement.HouseholdId))!.CanWrite.Should().BeFalse();
        typeof(FinancialMovement).GetProperty(nameof(movement.FinancialAccountId))!.CanWrite.Should().BeFalse();
        typeof(FinancialMovement).GetProperty(nameof(movement.Amount))!.CanWrite.Should().BeFalse();
        typeof(FinancialMovement).GetProperty(nameof(movement.MovementType))!.CanWrite.Should().BeFalse();
        typeof(FinancialMovement).GetProperty(nameof(movement.TransactionDate))!.CanWrite.Should().BeFalse();
        typeof(FinancialMovement).GetProperty(nameof(movement.EvidenceReference))!.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void Given_TwoIdsWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var value = Guid.NewGuid();

        var first = new FinancialMovementId(value);
        var second = new FinancialMovementId(value);

        first.Should().Be(second);
        (first == second).Should().BeTrue();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Fact]
    public void Given_TwoIdsWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new FinancialMovementId(Guid.NewGuid());
        var second = new FinancialMovementId(Guid.NewGuid());

        first.Should().NotBe(second);
        (first == second).Should().BeFalse();
    }

    [Fact]
    public void Given_NewIds_When_Creating_Then_TheyAreUnique()
    {
        var first = FinancialMovementId.New();
        var second = FinancialMovementId.New();

        first.Should().NotBe(default);
        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_AHouseholdId_When_AccessingValue_Then_ReturnsTheGuid()
    {
        var value = Guid.NewGuid();

        var householdId = new HouseholdId(value);

        householdId.Value.Should().Be(value);
    }

    [Fact]
    public void Given_TwoHouseholdIdsWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var value = Guid.NewGuid();

        var first = new HouseholdId(value);
        var second = new HouseholdId(value);

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoHouseholdIdsWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new HouseholdId(Guid.NewGuid());
        var second = new HouseholdId(Guid.NewGuid());

        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_AFinancialAccountId_When_AccessingValue_Then_ReturnsTheGuid()
    {
        var value = Guid.NewGuid();

        var financialAccountId = new FinancialAccountId(value);

        financialAccountId.Value.Should().Be(value);
    }

    [Fact]
    public void Given_TwoFinancialAccountIdsWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var value = Guid.NewGuid();

        var first = new FinancialAccountId(value);
        var second = new FinancialAccountId(value);

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoFinancialAccountIdsWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new FinancialAccountId(Guid.NewGuid());
        var second = new FinancialAccountId(Guid.NewGuid());

        first.Should().NotBe(second);
    }
}
