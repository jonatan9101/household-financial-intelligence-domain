using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public sealed record MovementType
{
    public string Name { get; }

    public MovementType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(DomainErrors.MovementType.CannotBeNullOrEmpty);
        }

        Name = name.Trim();
    }
}
