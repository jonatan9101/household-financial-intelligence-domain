namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed record HouseholdStatus
{
    public string Status { get; }

    private HouseholdStatus(string status)
    {
        Status = status;
    }

    public static HouseholdStatus Draft { get; } = new("Draft");

    public static HouseholdStatus Active { get; } = new("Active");

    public static HouseholdStatus Archived { get; } = new("Archived");

    public override string ToString() => Status;
}
