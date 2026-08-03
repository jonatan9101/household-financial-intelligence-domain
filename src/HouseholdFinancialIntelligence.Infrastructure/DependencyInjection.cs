using HouseholdFinancialIntelligence.Application.Persistence;
using HouseholdFinancialIntelligence.Domain.Repositories;
using HouseholdFinancialIntelligence.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdFinancialIntelligence.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<HouseholdFinancialDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IFinancialMovementRepository, FinancialMovementRepository>();
        services.AddScoped<ISaveChanges, SaveChanges>();

        return services;
    }
}
