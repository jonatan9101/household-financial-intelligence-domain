using HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;

namespace HouseholdFinancialIntelligence.Api.Endpoints;

public sealed record RegisterFinancialMovementRequest(
    Guid HouseholdId,
    Guid FinancialAccountId,
    decimal Amount,
    string? Currency,
    string? MovementType,
    DateOnly? TransactionDate,
    string? EvidenceReference,
    DateTimeOffset? OccurredAt);

public static class FinancialMovementsEndpoints
{
    public static void MapFinancialMovementsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/financial-movements", HandleRegisterAsync);
    }

    private static async Task<IResult> HandleRegisterAsync(
        RegisterFinancialMovementRequest? request,
        RegisterFinancialMovementService service,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return Results.Problem(detail: "Request body is required.", statusCode: StatusCodes.Status400BadRequest);
        }

        if (request.HouseholdId == Guid.Empty)
        {
            return Results.Problem(detail: "HouseholdId is required.", statusCode: StatusCodes.Status400BadRequest);
        }

        if (request.FinancialAccountId == Guid.Empty)
        {
            return Results.Problem(detail: "FinancialAccountId is required.", statusCode: StatusCodes.Status400BadRequest);
        }

        if (request.OccurredAt is null)
        {
            return Results.Problem(detail: "OccurredAt is required.", statusCode: StatusCodes.Status400BadRequest);
        }

        var command = new RegisterFinancialMovementCommand(
            new HouseholdId(request.HouseholdId),
            new FinancialAccountId(request.FinancialAccountId),
            request.Amount,
            request.Currency!,
            request.MovementType!,
            request.TransactionDate!.Value,
            request.EvidenceReference!,
            request.OccurredAt.Value);

        var id = await service.RegisterAsync(command, cancellationToken);

        return Results.Created($"/api/financial-movements/{id.Value}", new { id = id.Value });
    }
}
