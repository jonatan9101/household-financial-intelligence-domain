using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public sealed record MovementType
{
    public string Name { get; }

    public MovementType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("MovementType cannot be null or empty.");
        }

        Name = name.Trim();
    }
}
