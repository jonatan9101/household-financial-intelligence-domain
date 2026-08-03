using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HouseholdFinancialIntelligence.Infrastructure.Persistence;

public sealed class HouseholdFinancialDbContext : DbContext
{
    public HouseholdFinancialDbContext(DbContextOptions<HouseholdFinancialDbContext> options)
        : base(options)
    {
    }

    public DbSet<FinancialMovement> FinancialMovements => Set<FinancialMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FinancialMovementConfiguration());
    }
}
