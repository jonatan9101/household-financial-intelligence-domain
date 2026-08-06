namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;

public sealed record FinancialAccountRenamed(
    FinancialAccountId FinancialAccountId,
    AccountName AccountName,
    DateTimeOffset OccurredAt);