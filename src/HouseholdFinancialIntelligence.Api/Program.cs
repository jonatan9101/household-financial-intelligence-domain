using HouseholdFinancialIntelligence.Api.Endpoints;
using HouseholdFinancialIntelligence.Api.Middleware;
using HouseholdFinancialIntelligence.Application.UseCases.FinancialMovement.RegisterFinancialMovement;
using HouseholdFinancialIntelligence.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddScoped<RegisterFinancialMovementService>();

var app = builder.Build();

app.UseMiddleware<DomainExceptionMiddleware>();

app.MapGet("/", () => "Hello World!");
app.MapFinancialMovementsEndpoints();

app.Run();
