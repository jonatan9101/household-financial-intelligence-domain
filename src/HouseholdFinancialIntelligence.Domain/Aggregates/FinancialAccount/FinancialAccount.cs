using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;

public sealed class FinancialAccount : AggregateRoot<FinancialAccountId>
{
    private readonly List<object> _domainEvents = [];

    private FinancialAccount(
        FinancialAccountId id,
        HouseholdId householdId,
        AccountType accountType,
        AccountName accountName,
        AccountIdentifier accountIdentifier,
        Currency currency,
        InstitutionName? institution) : base(id)
    {
        HouseholdId = householdId;
        AccountType = accountType;
        AccountName = accountName;
        AccountIdentifier = accountIdentifier;
        Currency = currency;
        Institution = institution;
        Status = AccountStatus.Active;
    }

    public HouseholdId HouseholdId { get; }

    public AccountType AccountType { get; }

    public AccountName AccountName { get; }

    public AccountIdentifier AccountIdentifier { get; }

    public Currency Currency { get; }

    public InstitutionName? Institution { get; }

    public AccountStatus Status { get; }

    public IReadOnlyCollection<object> DomainEvents => _domainEvents;

    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static FinancialAccount Register(
        HouseholdId householdId,
        AccountType accountType,
        AccountName accountName,
        AccountIdentifier accountIdentifier,
        Currency currency,
        InstitutionName? institution,
        DateTimeOffset occurredAt)
    {
        var account = new FinancialAccount(
            FinancialAccountId.New(),
            householdId,
            accountType,
            accountName,
            accountIdentifier,
            currency,
            institution);

        account._domainEvents.Add(new FinancialAccountRegistered(
            account.Id,
            account.HouseholdId,
            account.AccountType,
            account.AccountName,
            account.AccountIdentifier,
            account.Currency,
            account.Institution,
            occurredAt));

        return account;
    }
}