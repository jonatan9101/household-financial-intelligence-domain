using HouseholdFinancialIntelligence.Application.Persistence;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Repositories;
using HouseholdFinancialIntelligence.Domain.SharedKernel;
using FinancialMovementAggregate = HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement.FinancialMovement;

namespace HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;

public sealed class RegisterFinancialMovementService
{
    private readonly IFinancialMovementRepository _repository;
    private readonly ISaveChanges _saveChanges;

    public RegisterFinancialMovementService(
        IFinancialMovementRepository repository,
        ISaveChanges saveChanges)
    {
        _repository = repository;
        _saveChanges = saveChanges;
    }

    public async Task<FinancialMovementId> RegisterAsync(
        RegisterFinancialMovementCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var evidenceReference = new EvidenceReference(command.EvidenceReference);

        if (await _repository.ExistsByEvidenceReferenceAsync(evidenceReference, cancellationToken))
        {
            throw new DomainException(
                DomainErrors.FinancialMovement.DuplicateMovementCode,
                DomainErrors.FinancialMovement.DuplicateMovement);
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

        await _saveChanges.SaveChangesAsync(cancellationToken);

        return movement.Id;
    }
}
