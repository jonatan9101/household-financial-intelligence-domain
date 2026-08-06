namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;

public sealed record FinancialAccountClosed(
    FinancialAccountId FinancialAccountId,
    DateTimeOffset OccurredAt);