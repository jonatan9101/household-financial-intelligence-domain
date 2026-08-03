using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;
using HouseholdFinancialIntelligence.Domain.SharedKernel;
using FinancialMovementAggregate = HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.FinancialMovement;

namespace HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

public sealed class RegisterFinancialMovementService
{
    private readonly IFinancialMovementRepository _repository;

    public RegisterFinancialMovementService(IFinancialMovementRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinancialMovementId> RegisterAsync(
        RegisterFinancialMovementCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var evidenceReference = new EvidenceReference(command.EvidenceReference);

        if (await _repository.ExistsByEvidenceReferenceAsync(evidenceReference, cancellationToken))
        {
            throw new DomainException(DomainErrors.FinancialMovement.DuplicateMovement);
        }

        var movement = FinancialMovementAggregate.Register(
            command.HouseholdId,
            command.FinancialAccountId,
            command.Amount,
            command.Currency,
            command.MovementType,
            command.TransactionDate,
            command.EvidenceReference,
            command.OccurredAt);

        await _repository.AddAsync(movement, cancellationToken);

        return movement.Id;
    }
}
