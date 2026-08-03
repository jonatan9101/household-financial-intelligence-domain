using HouseholdFinancialIntelligence.Application.Persistence;

namespace HouseholdFinancialIntelligence.Infrastructure.Persistence;

public sealed class SaveChanges : ISaveChanges
{
    private readonly HouseholdFinancialDbContext _context;

    public SaveChanges(HouseholdFinancialDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
