namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public sealed record Currency
{
    public string Code { get; }

    public Currency(string code)
    {
        if (code is null || code.Length != 3 || !code.All(char.IsAsciiLetter))
        {
            throw new DomainException(DomainErrors.Currency.InvalidIso4217Code);
        }

        Code = code.ToUpperInvariant();
    }
}
