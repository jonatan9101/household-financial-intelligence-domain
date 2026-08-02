namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public readonly record struct FinancialAccountId
{
    public Guid Value { get; }

    public FinancialAccountId(Guid value)
    {
        Value = value;
    }
}
