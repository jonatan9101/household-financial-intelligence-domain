using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class AggregateRootTests
{
    [Fact]
    public void Given_AnAggregateRoot_When_CreatingIt_Then_ItIsAnEntityWithIdentity()
    {
        var id = Guid.NewGuid();
        var aggregate = new TestAggregateRoot(id);

        aggregate.Should().BeAssignableTo<Entity<Guid>>();
        aggregate.Id.Should().Be(id);
    }

    [Fact]
    public void Given_TwoAggregatesWithSameId_When_ComparingEquality_Then_TheyAreEqual()
    {
        var id = Guid.NewGuid();
        var first = new TestAggregateRoot(id);
        var second = new TestAggregateRoot(id);

        first.Should().Be(second);
    }

    private sealed class TestAggregateRoot : AggregateRoot<Guid>
    {
        public TestAggregateRoot(Guid id) : base(id)
        {
        }
    }
}
