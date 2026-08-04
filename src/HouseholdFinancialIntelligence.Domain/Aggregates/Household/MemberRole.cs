namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed record MemberRole
{
    public string Role { get; }

    private MemberRole(string role)
    {
        Role = role;
    }

    public static MemberRole Owner { get; } = new("Owner");

    public static MemberRole Member { get; } = new("Member");

    public override string ToString() => Role;
}
