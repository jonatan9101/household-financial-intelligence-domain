namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public sealed record Money
{
    public decimal Amount { get; }

    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new DomainException("Money amount cannot be negative.");
        }

        if (currency is null)
        {
            throw new DomainException("Money requires a currency.");
        }

        Amount = amount;
        Currency = currency;
    }
}
