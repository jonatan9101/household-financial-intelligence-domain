namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public readonly record struct MemberId
{
    public Guid Value { get; }

    public MemberId(Guid value)
    {
        Value = value;
    }

    public static MemberId New() => new(Guid.NewGuid());
}
