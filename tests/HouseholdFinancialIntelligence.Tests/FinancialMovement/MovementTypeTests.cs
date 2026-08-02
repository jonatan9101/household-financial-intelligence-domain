using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class MovementTypeTests
{
    [Fact]
    public void Given_AValidName_When_Creating_Then_NameIsStored()
    {
        var movementType = new MovementType("Purchase");

        movementType.Name.Should().Be("Purchase");
    }

    [Fact]
    public void Given_WhitespaceAroundName_When_Creating_Then_ItIsTrimmed()
    {
        var movementType = new MovementType("  Income  ");

        movementType.Name.Should().Be("Income");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_MissingName_When_Creating_Then_DomainExceptionIsThrown(string? name)
    {
        var action = () => new MovementType(name!);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Given_ANameNotInAnyPredefinedList_When_Creating_Then_ItIsAccepted()
    {
        var movementType = new MovementType("SeasonalBonus");

        movementType.Name.Should().Be("SeasonalBonus");
    }

    [Fact]
    public void Given_TwoTypesWithSameName_When_Comparing_Then_TheyAreEqual()
    {
        var first = new MovementType("Purchase");
        var second = new MovementType("Purchase");

        first.Should().Be(second);
    }

    [Fact]
    public void Given_TwoTypesWithDifferentNames_When_Comparing_Then_TheyAreNotEqual()
    {
        var first = new MovementType("Purchase");
        var second = new MovementType("Income");

        first.Should().NotBe(second);
    }
}
