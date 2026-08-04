using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed class Member : Entity<MemberId>
{
    internal Member(MemberId id, MemberRole role) : base(id)
    {
        Role = role;
    }

    public MemberRole Role { get; internal set; }
}
