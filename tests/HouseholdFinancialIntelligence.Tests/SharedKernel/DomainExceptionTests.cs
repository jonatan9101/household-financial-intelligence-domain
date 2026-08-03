using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class DomainExceptionTests
{
    [Fact]
    public void Given_AMessage_When_CreatingDomainException_Then_MessageIsPreserved()
    {
        var exception = new DomainException("A business rule was violated.");

        exception.Message.Should().Be("A business rule was violated.");
    }

    [Fact]
    public void Given_ADomainException_When_Thrown_Then_ItIsDetectedAsABusinessFailure()
    {
        Action action = () => throw new DomainException("A business rule was violated.");

        action.Should().Throw<DomainException>().WithMessage("A business rule was violated.");
    }

    [Fact]
    public void Given_AMessage_When_CreatingDomainException_Then_ErrorCodeIsNull()
    {
        var exception = new DomainException("A business rule was violated.");

        exception.ErrorCode.Should().BeNull();
    }

    [Fact]
    public void Given_AnErrorCodeAndMessage_When_CreatingDomainException_Then_ErrorCodeIsPreserved()
    {
        var code = new DomainErrorCode("FM-001");

        var exception = new DomainException(code, "A movement already exists.");

        exception.ErrorCode.Should().Be(code);
        exception.Message.Should().Be("A movement already exists.");
    }
}
