using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed record AccountIdentifier
{
    public string Value { get; }

    public AccountIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.AccountIdentifier.Required);
        }

        Value = value.Trim();
    }
}