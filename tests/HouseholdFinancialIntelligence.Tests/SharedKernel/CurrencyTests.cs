using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class CurrencyTests
{
    [Fact]
    public void Given_AValidIsoCode_When_CreatingCurrency_Then_CodeIsStored()
    {
        var currency = new Currency("USD");

        currency.Code.Should().Be("USD");
    }

    [Fact]
    public void Given_LowercaseCode_When_CreatingCurrency_Then_ItIsNormalizedToUppercase()
    {
        var currency = new Currency("usd");

        currency.Code.Should().Be("USD");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("US")]
    [InlineData("USDE")]
    [InlineData("US1")]
    public void Given_InvalidCode_When_CreatingCurrency_Then_DomainExceptionIsThrown(string code)
    {
        var action = () => new Currency(code);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_NullCode_When_CreatingCurrency_Then_DomainExceptionIsThrown()
    {
        var action = () => new Currency(null!);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_TwoCurrenciesWithSameCode_When_Comparing_Then_TheyAreEqual()
    {
        var first = new Currency("USD");
        var second = new Currency("usd");

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoCurrenciesWithDifferentCodes_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new Currency("USD");
        var second = new Currency("EUR");

        first.Should().NotBe(second);
    }
}
