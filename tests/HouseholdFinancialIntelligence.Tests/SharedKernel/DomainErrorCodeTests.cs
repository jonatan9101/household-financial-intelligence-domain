using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class DomainErrorCodeTests
{
    [Fact]
    public void Given_AValidCode_When_CreatingDomainErrorCode_Then_CodeIsPreserved()
    {
        var code = new DomainErrorCode("FM-001");

        code.Code.Should().Be("FM-001");
    }

    [Fact]
    public void Given_AValidCode_When_CreatingDomainErrorCode_Then_ToStringReturnsTheCode()
    {
        var code = new DomainErrorCode("HM-001");

        code.ToString().Should().Be("HM-001");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("fm-001")]
    [InlineData("FM001")]
    [InlineData("FM-1")]
    [InlineData("FM-0011")]
    [InlineData("FM")]
    public void Given_AnInvalidCode_When_CreatingDomainErrorCode_Then_ArgumentExceptionIsThrown(string? code)
    {
        Action action = () => new DomainErrorCode(code!);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Given_TwoCodesWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var first = new DomainErrorCode("FM-001");
        var second = new DomainErrorCode("FM-001");

        first.Should().Be(second);
        (first == second).Should().BeTrue();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Fact]
    public void Given_TwoCodesWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new DomainErrorCode("FM-001");
        var second = new DomainErrorCode("FM-002");

        first.Should().NotBe(second);
    }
}
