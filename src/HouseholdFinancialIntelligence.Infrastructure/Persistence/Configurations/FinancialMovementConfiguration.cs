using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdFinancialIntelligence.Infrastructure.Persistence.Configurations;

public sealed class FinancialMovementConfiguration : IEntityTypeConfiguration<FinancialMovement>
{
    public void Configure(EntityTypeBuilder<FinancialMovement> builder)
    {
        builder.ToTable("FinancialMovement");

        builder.HasKey(fm => fm.Id);

        builder.Property(fm => fm.Id)
            .HasConversion(id => id.Value, value => new FinancialMovementId(value));

        builder.Property(fm => fm.HouseholdId)
            .HasConversion(id => id.Value, value => new HouseholdId(value));

        builder.Property(fm => fm.FinancialAccountId)
            .HasConversion(id => id.Value, value => new FinancialAccountId(value));

        builder.OwnsOne(fm => fm.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .HasColumnName("CurrencyCode")
                .HasMaxLength(3)
                .HasConversion(v => v.Code, v => new Currency(v));
        });

        builder.Navigation(fm => fm.Amount)
            .HasField("_amount")
            .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

        builder.Property(fm => fm.MovementType)
            .HasConversion(v => v.Name, v => new MovementType(v));

        builder.Property(fm => fm.TransactionDate)
            .HasConversion(v => v.Value, v => new TransactionDate(v));

        builder.Property(fm => fm.EvidenceReference)
            .HasConversion(v => v.Value, v => new EvidenceReference(v));

        builder.Ignore(fm => fm.DomainEvents);

        builder.HasIndex(fm => new { fm.HouseholdId, fm.EvidenceReference })
            .IsUnique()
            .HasDatabaseName("UX_FinancialMovement_Household_EvidenceReference");
    }
}
