using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed record HouseholdLocale
{
    public string Value { get; }

    public HouseholdLocale(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.HouseholdLocale.Required);
        }

        Value = value.Trim();
    }
}
