using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class EvidenceReferenceTests
{
    [Fact]
    public void Given_AValidReference_When_Creating_Then_ValueIsStored()
    {
        var evidenceReference = new EvidenceReference("receipt-2026-07-001");

        evidenceReference.Value.Should().Be("receipt-2026-07-001");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_MissingReference_When_Creating_Then_DomainExceptionIsThrown(string? reference)
    {
        var action = () => new EvidenceReference(reference!);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_ALongReference_When_Creating_Then_ItIsAccepted()
    {
        var longReference = new string('x', 1000);

        var evidenceReference = new EvidenceReference(longReference);

        evidenceReference.Value.Should().Be(longReference);
    }

    [Fact]
    public void Given_TwoReferencesWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var first = new EvidenceReference("receipt-2026-07-001");
        var second = new EvidenceReference("receipt-2026-07-001");

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoReferencesWithDifferentValues_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new EvidenceReference("receipt-2026-07-001");
        var second = new EvidenceReference("receipt-2026-07-002");

        first.Should().NotBe(second);
    }
}
