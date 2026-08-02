using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.SharedKernel;

public class EntityTests
{
    [Fact]
    public void Given_TwoEntitiesWithSameId_When_ComparingEquality_Then_TheyAreEqual()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity(id);
        var second = new TestEntity(id);

        first.Should().Be(second);
        (first == second).Should().BeTrue();
    }

    [Fact]
    public void Given_TwoEntitiesWithDifferentIds_When_ComparingEquality_Then_TheyAreNotEqual()
    {
        var first = new TestEntity(Guid.NewGuid());
        var second = new TestEntity(Guid.NewGuid());

        first.Should().NotBe(second);
        (first == second).Should().BeFalse();
    }

    [Fact]
    public void Given_AnEntity_When_AccessingItsId_Then_ReturnsTheSameIdentity()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);

        entity.Id.Should().Be(id);
    }

    [Fact]
    public void Given_TwoEntitiesWithDifferentIds_When_ComparingInequality_Then_TheyAreNotEqual()
    {
        var first = new TestEntity(Guid.NewGuid());
        var second = new TestEntity(Guid.NewGuid());

        (first != second).Should().BeTrue();
    }

    [Fact]
    public void Given_TwoEntitiesWithSameId_When_GettingHashCode_Then_TheyShareTheSameHash()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity(id);
        var second = new TestEntity(id);

        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Fact]
    public void Given_AnEntity_When_ComparedToNull_Then_ItIsNotEqual()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.Equals(null).Should().BeFalse();
        (entity == null).Should().BeFalse();
    }

    private sealed class TestEntity : Entity<Guid>
    {
        public TestEntity(Guid id) : base(id)
        {
        }
    }
}
