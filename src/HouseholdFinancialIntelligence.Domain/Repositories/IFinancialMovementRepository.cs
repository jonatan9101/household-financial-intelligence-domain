using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

namespace HouseholdFinancialIntelligence.Domain.Repositories;

public interface IFinancialMovementRepository
{
    Task<bool> ExistsByEvidenceReferenceAsync(
        EvidenceReference evidenceReference,
        CancellationToken cancellationToken);

    void Add(FinancialMovement financialMovement);
}
