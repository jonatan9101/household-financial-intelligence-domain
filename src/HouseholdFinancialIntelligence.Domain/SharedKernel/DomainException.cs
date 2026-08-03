namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public class DomainException : Exception
{
    public DomainErrorCode? ErrorCode { get; }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(DomainErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
