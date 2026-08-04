using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed record HouseholdName
{
    public string Value { get; }

    public HouseholdName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.HouseholdName.Required);
        }

        Value = value.Trim();
    }
}
