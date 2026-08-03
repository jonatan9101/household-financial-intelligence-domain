using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class TransactionDateTests
{
    [Fact]
    public void Given_AValidDate_When_Creating_Then_ValueIsStored()
    {
        var date = new DateOnly(2026, 7, 1);

        var transactionDate = new TransactionDate(date);

        transactionDate.Value.Should().Be(date);
    }

    [Fact]
    public void Given_ADefaultDate_When_Creating_Then_DomainExceptionIsThrown()
    {
        var action = () => new TransactionDate(default);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_AFutureDate_When_Creating_Then_ItIsAccepted()
    {
        var future = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

        var transactionDate = new TransactionDate(future);

        transactionDate.Value.Should().Be(future);
    }

    [Fact]
    public void Given_TwoDatesWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var first = new TransactionDate(new DateOnly(2026, 7, 1));
        var second = new TransactionDate(new DateOnly(2026, 7, 1));

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoDatesWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new TransactionDate(new DateOnly(2026, 7, 1));
        var second = new TransactionDate(new DateOnly(2026, 7, 2));

        first.Should().NotBe(second);
    }
}
