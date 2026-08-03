using HouseholdFinancialIntelligence.Application.Persistence;

namespace HouseholdFinancialIntelligence.Tests.RegisterFinancialMovement;

internal sealed class RecordingSaveChanges : ISaveChanges
{
    public List<string> CallLog { get; }

    public RecordingSaveChanges(List<string>? callLog = null)
    {
        CallLog = callLog ?? [];
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        CallLog.Add(nameof(SaveChangesAsync));
        return Task.CompletedTask;
    }
}
