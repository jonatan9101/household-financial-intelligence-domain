using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount;
using HouseholdFinancialIntelligence.Domain.Aggregates.FinancialAccount.Events;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class FinancialAccountTests
{
    private static readonly DateTimeOffset OccurredAt = new(2026, 8, 3, 10, 0, 0, TimeSpan.Zero);

    private static FinancialAccount RegisterValidAccount(InstitutionName? institution = null) =>
        FinancialAccount.Register(
            HouseholdId.New(),
            new AccountType("Checking"),
            new AccountName("Main Checking"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            institution,
            OccurredAt);

    [Fact]
    public void Given_ValidFacts_When_Registering_Then_AggregateReflectsThoseFacts()
    {
        var householdId = HouseholdId.New();
        var institution = new InstitutionName("Banco Bilbao Vizcaya Argentaria");

        var account = FinancialAccount.Register(
            householdId,
            new AccountType("Savings"),
            new AccountName("Ahorros"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            institution,
            OccurredAt);

        account.Id.Value.Should().NotBe(Guid.Empty);
        account.HouseholdId.Should().Be(householdId);
        account.AccountType.Should().Be(new AccountType("Savings"));
        account.AccountType.Value.Should().Be("Savings");
        account.AccountName.Should().Be(new AccountName("Ahorros"));
        account.AccountName.Value.Should().Be("Ahorros");
        account.AccountIdentifier.Should().Be(new AccountIdentifier("ES91 2100 0418 4502 0005 1332"));
        account.AccountIdentifier.Value.Should().Be("ES91 2100 0418 4502 0005 1332");
        account.Currency.Should().Be(new Currency("EUR"));
        account.Currency.Code.Should().Be("EUR");
        account.Institution.Should().Be(institution);
        account.Institution!.Value.Should().Be("Banco Bilbao Vizcaya Argentaria");
    }

    [Fact]
    public void Given_ValidFacts_When_Registering_Then_AccountIsActive()
    {
        var account = RegisterValidAccount();

        account.Status.Should().Be(AccountStatus.Active);
        account.Status.Status.Should().Be("Active");
    }

    [Fact]
    public void Given_ValidFacts_When_Registering_Then_AccountBelongsToTheHousehold()
    {
        var householdId = HouseholdId.New();

        var account = FinancialAccount.Register(
            householdId,
            new AccountType("Checking"),
            new AccountName("Main Checking"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            null,
            OccurredAt);

        account.HouseholdId.Should().Be(householdId);
    }

    [Fact]
    public void Given_ValidFacts_When_Registering_Then_ExactlyOneFinancialAccountRegisteredEventIsPublished()
    {
        var householdId = HouseholdId.New();

        var account = FinancialAccount.Register(
            householdId,
            new AccountType("Checking"),
            new AccountName("Main Checking"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            null,
            OccurredAt);

        account.DomainEvents.Should().HaveCount(1);
        var registered = account.DomainEvents.OfType<FinancialAccountRegistered>().Single();
        registered.FinancialAccountId.Should().Be(account.Id);
        registered.HouseholdId.Should().Be(householdId);
        registered.AccountType.Should().Be(account.AccountType);
        registered.AccountName.Should().Be(account.AccountName);
        registered.AccountIdentifier.Should().Be(account.AccountIdentifier);
        registered.Currency.Should().Be(account.Currency);
        registered.Institution.Should().BeNull();
        registered.OccurredAt.Should().Be(OccurredAt);
    }

    [Fact]
    public void Given_ValidFacts_When_RegisteringWithoutInstitution_Then_InstitutionIsNull()
    {
        var account = RegisterValidAccount();

        account.Institution.Should().BeNull();
    }

    [Fact]
    public void Given_ValidFacts_When_RegisteringWithInstitution_Then_InstitutionIsSet()
    {
        var institution = new InstitutionName("Bankia");

        var account = RegisterValidAccount(institution);

        account.Institution.Should().Be(institution);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankAccountName_When_Registering_Then_DomainExceptionIsThrown(string? accountName)
    {
        var action = () => FinancialAccount.Register(
            HouseholdId.New(),
            new AccountType("Checking"),
            new AccountName(accountName!),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            null,
            OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.AccountName.Required);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankAccountIdentifier_When_Registering_Then_DomainExceptionIsThrown(string? identifier)
    {
        var action = () => FinancialAccount.Register(
            HouseholdId.New(),
            new AccountType("Checking"),
            new AccountName("Main Checking"),
            new AccountIdentifier(identifier!),
            new Currency("EUR"),
            null,
            OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.AccountIdentifier.Required);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankAccountType_When_Registering_Then_DomainExceptionIsThrown(string? accountType)
    {
        var action = () => FinancialAccount.Register(
            HouseholdId.New(),
            new AccountType(accountType!),
            new AccountName("Main Checking"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            null,
            OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.AccountType.Required);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankInstitutionName_When_Registering_Then_DomainExceptionIsThrown(string? institution)
    {
        var action = () => FinancialAccount.Register(
            HouseholdId.New(),
            new AccountType("Checking"),
            new AccountName("Main Checking"),
            new AccountIdentifier("ES91 2100 0418 4502 0005 1332"),
            new Currency("EUR"),
            new InstitutionName(institution!),
            OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.InstitutionName.CannotBeBlank);
    }

    [Fact]
    public void Given_RegisteredAccount_When_InspectingFacts_Then_TheyAreImmutable()
    {
        var account = RegisterValidAccount();

        typeof(FinancialAccount).GetProperty(nameof(account.Id))!.CanWrite.Should().BeFalse();
        typeof(FinancialAccount).GetProperty(nameof(account.HouseholdId))!.CanWrite.Should().BeFalse();
        typeof(FinancialAccount).GetProperty(nameof(account.AccountType))!.CanWrite.Should().BeFalse();
        typeof(FinancialAccount).GetProperty(nameof(account.AccountIdentifier))!.CanWrite.Should().BeFalse();
        typeof(FinancialAccount).GetProperty(nameof(account.Currency))!.CanWrite.Should().BeFalse();
        typeof(FinancialAccount).GetProperty(nameof(account.Institution))!.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void Given_RegisteredAccount_When_InspectingStatus_SetIsNotPublic()
    {
        var property = typeof(FinancialAccount).GetProperty(nameof(FinancialAccount.Status))!;

        property.CanWrite.Should().BeTrue();
        property.SetMethod!.IsPublic.Should().BeFalse();
        property.GetMethod!.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Given_RegisteredAccount_When_InspectingAccountName_SetIsNotPublic()
    {
        var property = typeof(FinancialAccount).GetProperty(nameof(FinancialAccount.AccountName))!;

        property.CanWrite.Should().BeTrue();
        property.SetMethod!.IsPublic.Should().BeFalse();
        property.GetMethod!.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Given_ActiveAccount_When_Renaming_Then_AccountNameIsUpdated()
    {
        var account = RegisterValidAccount();
        var newName = new AccountName("Family Savings");

        account.Rename(newName, OccurredAt);

        account.AccountName.Should().Be(newName);
        account.AccountName.Value.Should().Be("Family Savings");
    }

    [Fact]
    public void Given_Account_When_Renaming_Then_SingleFinancialAccountRenamedEventIsPublished()
    {
        var account = RegisterValidAccount();

        account.Rename(new AccountName("Family Savings"), OccurredAt);

        var renamed = account.DomainEvents.OfType<FinancialAccountRenamed>().Single();
        renamed.FinancialAccountId.Should().Be(account.Id);
        renamed.AccountName.Should().Be(new AccountName("Family Savings"));
        renamed.OccurredAt.Should().Be(OccurredAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankAccountName_When_Renaming_Then_DomainExceptionIsThrown(string? accountName)
    {
        var account = RegisterValidAccount();

        var action = () => account.Rename(new AccountName(accountName!), OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.AccountName.Required);
    }

    [Fact]
    public void Given_AccountStatus_When_AccessingStatusAndToString_Then_ValuesAreExposed()
    {
        AccountStatus.Active.Status.Should().Be("Active");
        AccountStatus.Active.ToString().Should().Be("Active");
        AccountStatus.Closed.Status.Should().Be("Closed");
        AccountStatus.Closed.ToString().Should().Be("Closed");
    }

    [Fact]
    public void Given_NewFinancialAccountIds_When_Creating_Then_TheyAreUnique()
    {
        var first = FinancialAccountId.New();
        var second = FinancialAccountId.New();

        first.Should().NotBe(default);
        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_RegisteredAccount_When_ClearingDomainEvents_Then_NoEventsRemain()
    {
        var account = RegisterValidAccount();

        account.ClearDomainEvents();

        account.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Given_ActiveAccount_When_Closing_Then_StatusBecomesClosed()
    {
        var account = RegisterValidAccount();

        account.Close(OccurredAt);

        account.Status.Should().Be(AccountStatus.Closed);
        account.Status.Status.Should().Be("Closed");
    }

    [Fact]
    public void Given_ActiveAccount_When_Closing_Then_SingleFinancialAccountClosedEventIsPublished()
    {
        var account = RegisterValidAccount();

        account.Close(OccurredAt);

        var closed = account.DomainEvents.OfType<FinancialAccountClosed>().Single();
        closed.FinancialAccountId.Should().Be(account.Id);
        closed.OccurredAt.Should().Be(OccurredAt);
    }

    [Fact]
    public void Given_ActiveAccount_When_Closing_Then_IdentityAndMetadataAreUnchanged()
    {
        var account = RegisterValidAccount();
        var id = account.Id;
        var householdId = account.HouseholdId;
        var accountType = account.AccountType;
        var name = account.AccountName;
        var identifier = account.AccountIdentifier;
        var currency = account.Currency;

        account.Close(OccurredAt);

        account.Id.Should().Be(id);
        account.HouseholdId.Should().Be(householdId);
        account.AccountType.Should().Be(accountType);
        account.AccountName.Should().Be(name);
        account.AccountIdentifier.Should().Be(identifier);
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void Given_ClosedAccount_When_ClosingAgain_Then_DomainExceptionIsThrown()
    {
        var account = RegisterValidAccount();
        account.Close(OccurredAt);

        var action = () => account.Close(OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.FinancialAccount.CannotCloseExceptFromActive);
    }
}