namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public sealed record DomainErrorCode
{
    public string Code { get; }

    public DomainErrorCode(string code)
    {
        if (code is null || !System.Text.RegularExpressions.Regex.IsMatch(code, @"^[A-Z]{2,3}-\d{3}$"))
        {
            throw new ArgumentException("Domain error code must match the pattern 'XX-###' (e.g. FM-001).", nameof(code));
        }

        Code = code;
    }

    public override string ToString() => Code;
}
