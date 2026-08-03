using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public sealed record EvidenceReference
{
    public string Value { get; }

    public EvidenceReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.EvidenceReference.Required);
        }

        Value = value;
    }
}
