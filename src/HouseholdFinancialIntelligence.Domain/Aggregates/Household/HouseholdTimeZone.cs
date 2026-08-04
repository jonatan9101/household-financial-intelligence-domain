using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed record HouseholdTimeZone
{
    public string Value { get; }

    public HouseholdTimeZone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.HouseholdTimeZone.Required);
        }

        Value = value.Trim();
    }
}
