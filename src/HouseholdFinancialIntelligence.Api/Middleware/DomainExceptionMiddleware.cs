using HouseholdFinancialIntelligence.Domain.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace HouseholdFinancialIntelligence.Api.Middleware;

public sealed class DomainExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DomainExceptionMiddleware> _logger;

    public DomainExceptionMiddleware(RequestDelegate next, ILogger<DomainExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException exception)
        {
            var statusCode = exception.ErrorCode == DomainErrors.FinancialMovement.DuplicateMovementCode
                ? StatusCodes.Status409Conflict
                : StatusCodes.Status400BadRequest;

            await WriteProblemAsync(context, statusCode, exception.Message, exception.ErrorCode?.Code);
        }
        catch (BadHttpRequestException exception)
        {
            _logger.LogDebug(exception, "Invalid request body.");
            await WriteProblemAsync(
                context,
                StatusCodes.Status400BadRequest,
                "The request body is invalid or malformed.");
        }
        catch (DbUpdateException exception)
        {
            _logger.LogWarning(exception, "Database update failed while processing the request.");
            await WriteProblemAsync(
                context,
                StatusCodes.Status409Conflict,
                DomainErrors.FinancialMovement.DuplicateMovement,
                DomainErrors.FinancialMovement.DuplicateMovementCode.Code);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");
            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        int statusCode,
        string detail,
        string? code = null)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = detail
        };

        if (code is not null)
        {
            problem.Extensions["code"] = code;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    }
}
