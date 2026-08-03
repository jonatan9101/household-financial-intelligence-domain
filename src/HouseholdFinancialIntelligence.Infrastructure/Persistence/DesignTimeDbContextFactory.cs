using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HouseholdFinancialIntelligence.Infrastructure.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HouseholdFinancialDbContext>
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=hfi;Username=postgres;Password=postgres";

    public HouseholdFinancialDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<HouseholdFinancialDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new HouseholdFinancialDbContext(options);
    }
}
