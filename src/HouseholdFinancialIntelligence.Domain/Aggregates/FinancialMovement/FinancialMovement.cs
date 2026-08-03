using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.Events;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;

public sealed class FinancialMovement : AggregateRoot<FinancialMovementId>
{
    private readonly List<FinancialMovementRegistered> _domainEvents = [];

    private FinancialMovement(
        FinancialMovementId id,
        HouseholdId householdId,
        FinancialAccountId financialAccountId,
        Money amount,
        MovementType movementType,
        TransactionDate transactionDate,
        EvidenceReference evidenceReference) : base(id)
    {
        HouseholdId = householdId;
        FinancialAccountId = financialAccountId;
        Amount = amount;
        MovementType = movementType;
        TransactionDate = transactionDate;
        EvidenceReference = evidenceReference;
    }

    public HouseholdId HouseholdId { get; }

    public FinancialAccountId FinancialAccountId { get; }

    public Money Amount { get; }

    public MovementType MovementType { get; }

    public TransactionDate TransactionDate { get; }

    public EvidenceReference EvidenceReference { get; }

    public IReadOnlyCollection<FinancialMovementRegistered> DomainEvents => _domainEvents;

    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static FinancialMovement Register(
        HouseholdId householdId,
        FinancialAccountId financialAccountId,
        decimal amount,
        string currency,
        string movementType,
        DateOnly transactionDate,
        string evidenceReference,
        DateTimeOffset occurredAt)
    {
        var money = new Money(amount, new Currency(currency));
        var movementTypeValue = new MovementType(movementType);
        var transactionDateValue = new TransactionDate(transactionDate);
        var evidenceReferenceValue = new EvidenceReference(evidenceReference);

        if (money.Amount <= 0)
        {
            throw new DomainException(DomainErrors.FinancialMovement.AmountMustBeGreaterThanZero);
        }

        var movement = new FinancialMovement(
            FinancialMovementId.New(),
            householdId,
            financialAccountId,
            money,
            movementTypeValue,
            transactionDateValue,
            evidenceReferenceValue);

        movement._domainEvents.Add(new FinancialMovementRegistered(
            movement.Id,
            movement.HouseholdId,
            movement.FinancialAccountId,
            movement.Amount.Amount,
            movement.Amount.Currency,
            movement.MovementType,
            occurredAt));

        return movement;
    }
}
