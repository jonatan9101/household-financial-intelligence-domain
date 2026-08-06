namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;

public sealed record FinancialAccountReopened(
    FinancialAccountId FinancialAccountId,
    DateTimeOffset OccurredAt);