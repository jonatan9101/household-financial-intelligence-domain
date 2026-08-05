using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed record AccountName
{
    public string Value { get; }

    public AccountName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.AccountName.Required);
        }

        Value = value.Trim();
    }
}