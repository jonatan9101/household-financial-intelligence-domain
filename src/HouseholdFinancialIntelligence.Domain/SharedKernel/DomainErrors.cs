namespace HouseholdFinancialIntelligence.Domain.SharedKernel;

public static class DomainErrors
{
    public static class Money
    {
        public const string AmountCannotBeNegative = "Money amount cannot be negative.";
        public const string CurrencyRequired = "Money requires a currency.";
    }

    public static class Currency
    {
        public const string InvalidIso4217Code = "Currency must be a 3-letter ISO 4217 code.";
    }

    public static class FinancialMovement
    {
        public static readonly DomainErrorCode DuplicateMovementCode = new("FM-001");
        public const string AmountMustBeGreaterThanZero = "Amount must be greater than zero.";
        public const string DuplicateMovement = "A movement with the same evidence reference already exists.";
    }

    public static class MovementType
    {
        public const string CannotBeNullOrEmpty = "MovementType cannot be null or empty.";
    }

    public static class TransactionDate
    {
        public const string Required = "TransactionDate is required.";
    }

    public static class EvidenceReference
    {
        public const string Required = "EvidenceReference is required.";
    }
}
