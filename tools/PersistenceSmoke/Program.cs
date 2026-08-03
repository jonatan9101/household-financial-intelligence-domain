using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialMovement;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var failed = false;

void Check(bool condition, string step)
{
    Console.WriteLine($"[{(condition ? "PASS" : "FAIL")}] {step}");
    if (!condition)
    {
        failed = true;
    }
}

var factory = new DesignTimeDbContextFactory();
using var context = factory.CreateDbContext([]);

Console.WriteLine("Applying migrations...");
await context.Database.MigrateAsync();
Check(true, "Migration applied");

await context.FinancialMovements.ExecuteDeleteAsync();

var householdId = new HouseholdId(Guid.NewGuid());
var accountId = new FinancialAccountId(Guid.NewGuid());
var evidence = "evt-" + Guid.NewGuid().ToString("N");
var occurredAt = new DateTimeOffset(2026, 8, 1, 10, 30, 0, TimeSpan.Zero);

var movement = FinancialMovement.Register(
    householdId,
    accountId,
    1234.56m,
    "USD",
    "salary",
    new DateOnly(2026, 8, 1),
    evidence,
    occurredAt);

var repository = new FinancialMovementRepository(context);

var existsBefore = await repository.ExistsByEvidenceReferenceAsync(
    new EvidenceReference(evidence),
    CancellationToken.None);
Check(!existsBefore, "ExistsByEvidenceReferenceAsync returns false before AddAsync");

await repository.AddAsync(movement, CancellationToken.None);
await context.SaveChangesAsync();
Check(true, "SaveChangesAsync completed explicitly outside the repository");

var stored = await context.FinancialMovements.SingleAsync(fm => fm.Id == movement.Id);
Check(stored.Id == movement.Id, "FinancialMovementId round-trip");
Check(stored.HouseholdId == householdId, "HouseholdId round-trip");
Check(stored.FinancialAccountId == accountId, "FinancialAccountId round-trip");
Check(stored.Amount.Amount == 1234.56m, "Amount round-trip");
Check(stored.Amount.Currency.Code == "USD", "Currency round-trip");
Check(stored.MovementType.Name == "salary", "MovementType round-trip");
Check(stored.TransactionDate.Value == new DateOnly(2026, 8, 1), "TransactionDate round-trip");
Check(stored.EvidenceReference.Value == evidence, "EvidenceReference round-trip");

var existsAfter = await repository.ExistsByEvidenceReferenceAsync(
    new EvidenceReference(evidence),
    CancellationToken.None);
Check(existsAfter, "ExistsByEvidenceReferenceAsync returns true after save");

var duplicate = FinancialMovement.Register(
    householdId,
    accountId,
    50.00m,
    "EUR",
    "other",
    new DateOnly(2026, 8, 2),
    evidence,
    occurredAt);

await repository.AddAsync(duplicate, CancellationToken.None);

try
{
    await context.SaveChangesAsync();
    Check(false, "Unique index rejects duplicate (no exception thrown)");
}
catch (DbUpdateException)
{
    Check(true, "Unique index rejects duplicate (DbUpdateException)");
}

Console.WriteLine(failed ? "SMOKE FAILED" : "SMOKE PASSED");
Environment.ExitCode = failed ? 1 : 0;
