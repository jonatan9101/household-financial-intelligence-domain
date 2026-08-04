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

    public static class HouseholdName
    {
        public const string Required = "Household name is required.";
    }

    public static class HouseholdTimeZone
    {
        public const string Required = "Household timezone is required.";
    }

    public static class HouseholdLocale
    {
        public const string Required = "Household locale is required.";
    }

    public static class Household
    {
        public const string BaseCurrencyCannotBeChangedOutsideDraft = "BaseCurrency can only be set while the Household is in Draft.";
        public const string CannotActivateExceptFromDraftState = "Only a Household in Draft can be activated.";
        public const string BaseCurrencyRequiredToActivate = "A Household must have a BaseCurrency defined to be activated.";
        public const string ExactlyOneOwnerRequiredToActivate = "A Household must have exactly one Owner to be activated.";
        public const string CannotJoinArchivedHousehold = "An archived Household cannot accept new members.";
        public const string DuplicateMember = "A member with that identity already belongs to the Household.";
        public const string DuplicateOwner = "A Household can have exactly one Owner.";
        public const string MemberNotFound = "The member does not belong to the Household.";
        public const string CannotRemoveLastOwner = "The last Owner of a Household cannot be removed.";
        public const string CannotRemoveOwnerRoleFromLastOwner = "The Owner role cannot be removed from the last Owner.";
        public const string CannotArchiveExceptFromActiveState = "Only an active Household can be archived.";
        public const string OnlyOwnerCanArchive = "Only the Owner can archive a Household.";
    }
}
