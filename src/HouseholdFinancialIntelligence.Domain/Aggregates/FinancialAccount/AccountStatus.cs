namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed record AccountStatus
{
    public string Status { get; }

    private AccountStatus(string status)
    {
        Status = status;
    }

    public static AccountStatus Active { get; } = new("Active");

    public static AccountStatus Closed { get; } = new("Closed");

    public override string ToString() => Status;
}