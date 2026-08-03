using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public sealed record TransactionDate
{
    public DateOnly Value { get; }

    public TransactionDate(DateOnly value)
    {
        if (value == default)
        {
            throw new DomainException(DomainErrors.TransactionDate.Required);
        }

        Value = value;
    }
}
