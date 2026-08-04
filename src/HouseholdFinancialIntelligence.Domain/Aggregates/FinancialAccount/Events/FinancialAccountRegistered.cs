using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;

public sealed record FinancialAccountRegistered(
    FinancialAccountId FinancialAccountId,
    HouseholdId HouseholdId,
    AccountType AccountType,
    AccountName AccountName,
    AccountIdentifier AccountIdentifier,
    Currency Currency,
    InstitutionName? Institution,
    DateTimeOffset OccurredAt);