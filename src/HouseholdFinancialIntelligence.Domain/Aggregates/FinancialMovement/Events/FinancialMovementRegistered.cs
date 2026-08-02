using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.Events;

public sealed record FinancialMovementRegistered(
    FinancialMovementId FinancialMovementId,
    HouseholdId HouseholdId,
    FinancialAccountId FinancialAccountId,
    decimal Amount,
    Currency Currency,
    MovementType MovementType,
    DateTimeOffset OccurredAt);
