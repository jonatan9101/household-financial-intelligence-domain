using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household.Events;

public sealed record HouseholdActivated(
    HouseholdId HouseholdId,
    Currency BaseCurrency,
    DateTimeOffset OccurredAt);
