using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed record InstitutionName
{
    public string Value { get; }

    public InstitutionName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.InstitutionName.CannotBeBlank);
        }

        Value = value.Trim();
    }
}