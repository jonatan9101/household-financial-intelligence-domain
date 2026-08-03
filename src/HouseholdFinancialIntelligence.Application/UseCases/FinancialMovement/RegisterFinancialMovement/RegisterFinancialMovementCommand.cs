using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;

namespace HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

public sealed record RegisterFinancialMovementCommand(
    HouseholdId HouseholdId,
    FinancialAccountId FinancialAccountId,
    decimal Amount,
    string Currency,
    string MovementType,
    DateOnly TransactionDate,
    string EvidenceReference,
    DateTimeOffset OccurredAt);
