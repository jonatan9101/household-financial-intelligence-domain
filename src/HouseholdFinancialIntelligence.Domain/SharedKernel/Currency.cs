namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public sealed record Currency
{
    public string Code { get; }

    public Currency(string code)
    {
        if (code is null || code.Length != 3 || !code.All(char.IsAsciiLetter))
        {
            throw new DomainException("Currency must be a 3-letter ISO 4217 code.");
        }

        Code = code.ToUpperInvariant();
    }
}
