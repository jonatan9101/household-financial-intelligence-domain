using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household.Events;

public sealed record HouseholdArchived(
    HouseholdId HouseholdId,
    MemberId ArchivedBy,
    DateTimeOffset OccurredAt);
