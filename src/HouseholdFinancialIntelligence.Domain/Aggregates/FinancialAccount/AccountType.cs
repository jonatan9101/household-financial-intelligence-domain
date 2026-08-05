using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed record AccountType
{
    public string Value { get; }

    public AccountType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.AccountType.Required);
        }

        Value = value.Trim();
    }
}