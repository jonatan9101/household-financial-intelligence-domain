namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public readonly record struct FinancialMovementId
{
    public Guid Value { get; }

    public FinancialMovementId(Guid value)
    {
        Value = value;
    }

    public static FinancialMovementId New() => new(Guid.NewGuid());
}
