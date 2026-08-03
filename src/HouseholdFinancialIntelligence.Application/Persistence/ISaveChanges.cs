namespace HouseholdFinancialIntelligence.Application.Persistence;

public interface ISaveChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
