namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public sealed record Money
{
    public decimal Amount { get; }

    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new DomainException(DomainErrors.Money.AmountCannotBeNegative);
        }

        if (currency is null)
        {
            throw new DomainException(DomainErrors.Money.CurrencyRequired);
        }

        Amount = amount;
        Currency = currency;
    }
}
