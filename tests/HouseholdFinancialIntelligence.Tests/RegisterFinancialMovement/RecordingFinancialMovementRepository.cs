using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;

namespace HouseholdFinancialIntelligence.Tests.RegisterFinancialMovement;

internal sealed class RecordingFinancialMovementRepository : IFinancialMovementRepository
{
    private readonly HashSet<EvidenceReference> _existingEvidenceReferences = [];

    public List<string> CallLog { get; }

    public RecordingFinancialMovementRepository(List<string>? callLog = null)
    {
        CallLog = callLog ?? [];
    }

    public FinancialMovement? Added { get; private set; }

    public void Seed(EvidenceReference evidenceReference) => _existingEvidenceReferences.Add(evidenceReference);

    public Task<bool> ExistsByEvidenceReferenceAsync(
        EvidenceReference evidenceReference,
        CancellationToken cancellationToken)
    {
        CallLog.Add(nameof(ExistsByEvidenceReferenceAsync));
        return Task.FromResult(_existingEvidenceReferences.Contains(evidenceReference));
    }

    public Task AddAsync(FinancialMovement financialMovement, CancellationToken cancellationToken)
    {
        CallLog.Add(nameof(AddAsync));
        Added = financialMovement;
        return Task.CompletedTask;
    }
}
