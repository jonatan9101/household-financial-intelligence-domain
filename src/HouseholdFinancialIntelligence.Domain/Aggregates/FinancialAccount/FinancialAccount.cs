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

    public AccountName AccountName { get; private set; }

    public AccountIdentifier AccountIdentifier { get; }

    public Currency Currency { get; }

    public InstitutionName? Institution { get; }

    public AccountStatus Status { get; private set; }

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

    public void Rename(AccountName newName, DateTimeOffset occurredAt)
    {
        AccountName = newName;

        _domainEvents.Add(new FinancialAccountRenamed(
            Id,
            newName,
            occurredAt));
    }

    public void Close(DateTimeOffset occurredAt)
    {
        if (Status != AccountStatus.Active)
        {
            throw new DomainException(DomainErrors.FinancialAccount.CannotCloseExceptFromActive);
        }

        Status = AccountStatus.Closed;

        _domainEvents.Add(new FinancialAccountClosed(
            Id,
            occurredAt));
    }

    public void Reopen(DateTimeOffset occurredAt)
    {
        if (Status != AccountStatus.Closed)
        {
            throw new DomainException(DomainErrors.FinancialAccount.CannotReopenExceptFromClosed);
        }

        Status = AccountStatus.Active;

        _domainEvents.Add(new FinancialAccountReopened(
            Id,
            occurredAt));
    }
}