using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HouseholdFinancialIntelligence.Infrastructure.Persistence;

public sealed class FinancialMovementRepository : IFinancialMovementRepository
{
    private readonly HouseholdFinancialDbContext _context;

    public FinancialMovementRepository(HouseholdFinancialDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEvidenceReferenceAsync(
        EvidenceReference evidenceReference,
        CancellationToken cancellationToken)
    {
        return await _context.FinancialMovements.AnyAsync(
            fm => fm.EvidenceReference == evidenceReference,
            cancellationToken);
    }

    public async Task AddAsync(
        FinancialMovement financialMovement,
        CancellationToken cancellationToken)
    {
        await _context.FinancialMovements.AddAsync(financialMovement, cancellationToken);
    }
}
