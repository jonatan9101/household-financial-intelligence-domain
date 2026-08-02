using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class MoneyTests
{
    private static readonly Currency Usd = new("USD");

    [Fact]
    public void Given_ValidAmountAndCurrency_When_CreatingMoney_Then_AmountAndCurrencyAreSet()
    {
        var money = new Money(100.50m, Usd);

        money.Amount.Should().Be(100.50m);
        money.Currency.Should().Be(Usd);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(999.99)]
    public void Given_NonNegativeAmount_When_CreatingMoney_Then_ItIsValid(decimal amount)
    {
        var money = new Money(amount, Usd);

        money.Amount.Should().Be(amount);
    }

    [Fact]
    public void Given_NegativeAmount_When_CreatingMoney_Then_DomainExceptionIsThrown()
    {
        var action = () => new Money(-0.01m, Usd);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_NullCurrency_When_CreatingMoney_Then_DomainExceptionIsThrown()
    {
        var action = () => new Money(10m, null!);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_TwoMoneysWithSameAmountAndCurrency_When_Comparing_Then_TheyAreEqual()
    {
        var first = new Money(10m, Usd);
        var second = new Money(10m, Usd);

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoMoneysWithDifferentAmounts_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new Money(10m, Usd);
        var second = new Money(20m, Usd);

        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_TwoMoneysWithDifferentCurrencies_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new Money(10m, Usd);
        var second = new Money(10m, new Currency("EUR"));

        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_Money_When_InspectingProperties_Then_TheyCannotBeModified()
    {
        typeof(Money).GetProperty(nameof(Money.Amount))!.CanWrite.Should().BeFalse();
        typeof(Money).GetProperty(nameof(Money.Currency))!.CanWrite.Should().BeFalse();
    }
}
