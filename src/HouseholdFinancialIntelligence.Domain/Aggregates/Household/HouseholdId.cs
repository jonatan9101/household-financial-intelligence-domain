namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public readonly record struct HouseholdId
{
    public Guid Value { get; }

    public HouseholdId(Guid value)
    {
        Value = value;
    }
}
