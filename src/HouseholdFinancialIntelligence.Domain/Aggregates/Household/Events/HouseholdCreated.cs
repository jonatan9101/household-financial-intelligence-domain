namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household.Events;

public sealed record HouseholdCreated(
    HouseholdId HouseholdId,
    HouseholdName Name,
    HouseholdTimeZone TimeZone,
    HouseholdLocale Locale,
    DateTimeOffset OccurredAt);
