using FluentAssertions;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household;
using HouseholdFinancialIntelligence.Domain.Aggregates.Household.Events;
using HouseholdFinancialIntelligence.Domain.SharedKernel;
using System.Reflection;

namespace HouseholdFinancialIntelligence.Tests.Aggregates;

public class HouseholdTests
{
    private static readonly DateTimeOffset OccurredAt = new(2026, 8, 3, 10, 0, 0, TimeSpan.Zero);

    private static Household CreateValidHousehold(MemberId? ownerId = null) =>
        Household.Create(
            "Mi Hogar",
            "America/Bogota",
            "es-CO",
            ownerId ?? MemberId.New(),
            OccurredAt);

    private static void RemoveAllMembers(Household household)
    {
        var field = typeof(Household).GetField("_members", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var members = (List<Member>)field.GetValue(household)!;
        members.Clear();
    }

    [Fact]
    public void Given_ValidFacts_When_Creating_Then_AggregateReflectsThoseFacts()
    {
        var name = "Mi Hogar";

        var household = Household.Create(name, "America/Bogota", "es-CO", MemberId.New(), OccurredAt);

        household.Id.Value.Should().NotBe(Guid.Empty);
        household.Name.Should().Be(new HouseholdName(name));
        household.Name.Value.Should().Be(name);
        household.TimeZone.Should().Be(new HouseholdTimeZone("America/Bogota"));
        household.TimeZone.Value.Should().Be("America/Bogota");
        household.Locale.Should().Be(new HouseholdLocale("es-CO"));
        household.Locale.Value.Should().Be("es-CO");
        household.Status.Should().Be(HouseholdStatus.Draft);
        household.Status.Status.Should().Be("Draft");
        household.BaseCurrency.Should().BeNull();
    }

    [Fact]
    public void Given_ValidFacts_When_Creating_Then_ExactlyOneOwnerExists()
    {
        var ownerId = MemberId.New();

        var household = Household.Create("Mi Hogar", "America/Bogota", "es-CO", ownerId, OccurredAt);

        household.Members.Should().HaveCount(1);
        var owner = household.Members.Single();
        owner.Id.Should().Be(ownerId);
        owner.Id.Value.Should().Be(ownerId.Value);
        owner.Role.Should().Be(MemberRole.Owner);
        owner.Role.Role.Should().Be("Owner");
    }

    [Fact]
    public void Given_ValidCreation_When_Creating_Then_ExactlyOneHouseholdCreatedEventIsPublished()
    {
        var household = CreateValidHousehold();

        household.DomainEvents.Should().HaveCount(1);
        var created = household.DomainEvents.OfType<HouseholdCreated>().Single();
        created.HouseholdId.Should().Be(household.Id);
        created.Name.Should().Be(household.Name);
        created.TimeZone.Should().Be(household.TimeZone);
        created.Locale.Should().Be(household.Locale);
        created.OccurredAt.Should().Be(OccurredAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankName_When_Creating_Then_DomainExceptionIsThrown(string? name)
    {
        var action = () => Household.Create(name!, "America/Bogota", "es-CO", MemberId.New(), OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.HouseholdName.Required);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankTimeZone_When_Creating_Then_DomainExceptionIsThrown(string? timeZone)
    {
        var action = () => Household.Create("Mi Hogar", timeZone!, "es-CO", MemberId.New(), OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.HouseholdTimeZone.Required);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Given_BlankLocale_When_Creating_Then_DomainExceptionIsThrown(string? locale)
    {
        var action = () => Household.Create("Mi Hogar", "America/Bogota", locale!, MemberId.New(), OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.HouseholdLocale.Required);
    }

    [Fact]
    public void Given_CreatedHousehold_When_InspectingFacts_Then_TheyAreImmutable()
    {
        var household = CreateValidHousehold();

        typeof(Household).GetProperty(nameof(household.Id))!.CanWrite.Should().BeFalse();
        typeof(Household).GetProperty(nameof(household.Name))!.CanWrite.Should().BeFalse();
        typeof(Household).GetProperty(nameof(household.TimeZone))!.CanWrite.Should().BeFalse();
        typeof(Household).GetProperty(nameof(household.Locale))!.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void Given_CreatedHousehold_When_ClearingDomainEvents_Then_NoEventsRemain()
    {
        var household = CreateValidHousehold();

        household.ClearDomainEvents();

        household.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Given_NewMemberIds_When_Creating_Then_TheyAreUnique()
    {
        var first = MemberId.New();
        var second = MemberId.New();

        first.Should().NotBe(default);
        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_NewHouseholdIds_When_Creating_Then_TheyAreUnique()
    {
        var first = HouseholdId.New();
        var second = HouseholdId.New();

        first.Should().NotBe(default);
        first.Should().NotBe(second);
    }

    [Fact]
    public void Given_TwoMemberIdsWithSameValue_When_Comparing_Then_TheyAreEqual()
    {
        var value = Guid.NewGuid();

        var first = new MemberId(value);
        var second = new MemberId(value);

        first.Should().Be(second);
    }

    [Fact]
    public void Given_MemberRole_When_AccessingRoleAndToString_Then_ValuesAreExposed()
    {
        MemberRole.Owner.Role.Should().Be("Owner");
        MemberRole.Owner.ToString().Should().Be("Owner");
        MemberRole.Member.Role.Should().Be("Member");
        MemberRole.Member.ToString().Should().Be("Member");
    }

    [Fact]
    public void Given_HouseholdStatus_When_AccessingStatusAndToString_Then_ValuesAreExposed()
    {
        HouseholdStatus.Draft.Status.Should().Be("Draft");
        HouseholdStatus.Draft.ToString().Should().Be("Draft");
        HouseholdStatus.Active.Status.Should().Be("Active");
        HouseholdStatus.Active.ToString().Should().Be("Active");
        HouseholdStatus.Archived.Status.Should().Be("Archived");
        HouseholdStatus.Archived.ToString().Should().Be("Archived");
    }

    [Fact]
    public void Given_DraftHousehold_When_SetBaseCurrency_Then_BaseCurrencyIsSet()
    {
        var household = CreateValidHousehold();

        household.SetBaseCurrency("USD");

        household.BaseCurrency.Should().Be(new Currency("USD"));
        household.BaseCurrency!.Code.Should().Be("USD");
    }

    [Fact]
    public void Given_DraftHousehold_When_ChangingBaseCurrency_Then_ItIsOverwritten()
    {
        var household = CreateValidHousehold();

        household.SetBaseCurrency("USD");
        household.SetBaseCurrency("EUR");

        household.BaseCurrency.Should().Be(new Currency("EUR"));
    }

    [Fact]
    public void Given_DraftHousehold_When_SetBaseCurrency_WithInvalidCurrency_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();

        var action = () => household.SetBaseCurrency("XX");

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Currency.InvalidIso4217Code);
    }

    [Fact]
    public void Given_ActiveHousehold_When_SetBaseCurrency_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);

        var action = () => household.SetBaseCurrency("EUR");

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.BaseCurrencyCannotBeChangedOutsideDraft);
        household.BaseCurrency.Should().Be(new Currency("USD"));
    }

    [Fact]
    public void Given_ArchivedHousehold_When_SetBaseCurrency_Then_DomainExceptionIsThrown()
    {
        var household = CreateArchivedHousehold();

        var action = () => household.SetBaseCurrency("USD");

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.BaseCurrencyCannotBeChangedOutsideDraft);
        household.BaseCurrency.Should().Be(new Currency("USD"));
    }

    [Fact]
    public void Given_DraftHousehold_When_SetBaseCurrency_Then_NoDomainEventIsPublished()
    {
        var household = CreateValidHousehold();

        household.SetBaseCurrency("USD");

        household.DomainEvents.OfType<HouseholdCreated>().Should().HaveCount(1);
        household.DomainEvents.Should().NotContainItemsAssignableTo<HouseholdActivated>();
    }

    [Fact]
    public void Given_DraftHousehold_WithBaseCurrency_When_Activate_Then_StatusBecomesActive()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");

        household.Activate(OccurredAt);

        household.Status.Should().Be(HouseholdStatus.Active);
        household.BaseCurrency.Should().Be(new Currency("USD"));
        household.Members.Should().HaveCount(1);
        household.BaseCurrency!.Code.Should().Be("USD");
    }

    [Fact]
    public void Given_DraftHousehold_When_Activate_Then_ExactlyOneHouseholdActivatedEventIsPublished()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");

        household.Activate(OccurredAt);

        household.DomainEvents.OfType<HouseholdActivated>().Should().HaveCount(1);
        var activated = household.DomainEvents.OfType<HouseholdActivated>().Single();
        activated.HouseholdId.Should().Be(household.Id);
        activated.BaseCurrency.Should().Be(new Currency("USD"));
        activated.OccurredAt.Should().Be(OccurredAt);
    }

    [Fact]
    public void Given_ActiveHousehold_When_Activate_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);

        var action = () => household.Activate(OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotActivateExceptFromDraftState);
    }

    [Fact]
    public void Given_ArchivedHousehold_When_Activate_Then_DomainExceptionIsThrown()
    {
        var household = CreateArchivedHousehold();

        var action = () => household.Activate(OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotActivateExceptFromDraftState);
    }

    [Fact]
    public void Given_DraftHousehold_WithoutBaseCurrency_When_Activate_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();

        var action = () => household.Activate(OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.BaseCurrencyRequiredToActivate);
        household.Status.Should().Be(HouseholdStatus.Draft);
    }

    [Fact]
    public void Given_DraftHousehold_WithoutAnOwner_When_Activate_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        RemoveAllMembers(household);

        var action = () => household.Activate(OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.ExactlyOneOwnerRequiredToActivate);
        household.Status.Should().Be(HouseholdStatus.Draft);
    }

    [Fact]
    public void Given_ActiveHousehold_When_AddMember_Then_MemberBelongsToTheHousehold()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var memberId = MemberId.New();

        household.AddMember(memberId, MemberRole.Member);

        household.Members.Should().HaveCount(2);
        var added = household.Members.Single(member => member.Id == memberId);
        added.Id.Should().Be(memberId);
        added.Role.Should().Be(MemberRole.Member);
        added.Role.Role.Should().Be("Member");
    }

    [Fact]
    public void Given_DraftHousehold_When_AddMember_Then_MemberBelongsToTheHousehold()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();

        household.AddMember(memberId, MemberRole.Member);

        household.Members.Should().HaveCount(2);
        household.Members.Should().Contain(member => member.Id == memberId);
    }

    [Fact]
    public void Given_ArchivedHousehold_When_AddMember_Then_DomainExceptionIsThrown()
    {
        var household = CreateArchivedHousehold();

        var action = () => household.AddMember(MemberId.New(), MemberRole.Member);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotJoinArchivedHousehold);
        household.Members.Should().HaveCount(1);
    }

    [Fact]
    public void Given_Household_When_AddingDuplicateMember_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();

        household.AddMember(memberId, MemberRole.Member);

        var action = () => household.AddMember(memberId, MemberRole.Member);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.DuplicateMember);
        household.Members.Should().HaveCount(2);
    }

    [Fact]
    public void Given_Household_When_AddingASecondOwner_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var newOwnerId = MemberId.New();

        var action = () => household.AddMember(newOwnerId, MemberRole.Owner);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.DuplicateOwner);
        household.Members.Should().HaveCount(1);
    }

    [Fact]
    public void Given_Household_When_AddMember_Then_NoDomainEventIsPublished()
    {
        var household = CreateValidHousehold();

        household.AddMember(MemberId.New(), MemberRole.Member);

        household.DomainEvents.OfType<HouseholdCreated>().Should().HaveCount(1);
        household.DomainEvents.Should().NotContainItemsAssignableTo<HouseholdActivated>();
    }

    [Fact]
    public void Given_HouseholdWithMembers_When_RemoveMember_Then_MemberIsRemoved()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        household.RemoveMember(memberId);

        household.Members.Should().HaveCount(1);
        household.Members.Should().NotContain(member => member.Id == memberId);
        household.Members.Single().Role.Should().Be(MemberRole.Owner);
    }

    [Fact]
    public void Given_HouseholdWithMembers_When_RemoveMember_Then_OnlyMembershipIsAffected()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        household.RemoveMember(memberId);

        household.Id.Value.Should().NotBe(Guid.Empty);
        household.Name.Should().Be(new HouseholdName("Mi Hogar"));
        household.BaseCurrency.Should().Be(new Currency("USD"));
        household.Status.Should().Be(HouseholdStatus.Active);
        household.Members.Should().HaveCount(1);
    }

    [Fact]
    public void Given_Household_When_RemovingUnknownMember_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();

        var action = () => household.RemoveMember(MemberId.New());

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.MemberNotFound);
        household.Members.Should().HaveCount(1);
    }

    [Fact]
    public void Given_Household_When_RemovingLastOwner_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var ownerId = household.Members.Single().Id;

        var action = () => household.RemoveMember(ownerId);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotRemoveLastOwner);
        household.Members.Should().HaveCount(1);
        household.Members.Single().Role.Should().Be(MemberRole.Owner);
    }

    [Fact]
    public void Given_Household_When_RemoveMember_Then_NoDomainEventIsPublished()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        household.RemoveMember(memberId);

        household.DomainEvents.OfType<HouseholdCreated>().Should().HaveCount(1);
        household.DomainEvents.Should().NotContainItemsAssignableTo<HouseholdActivated>();
    }

    [Fact]
    public void Given_Member_When_ChangeMemberRoleToSameRole_Then_RoleIsUnchanged()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        household.ChangeMemberRole(memberId, MemberRole.Member);

        household.Members.Single(member => member.Id == memberId).Role.Should().Be(MemberRole.Member);
    }

    [Fact]
    public void Given_HouseholdWithAnOwner_When_PromotingMemberToOwner_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        var action = () => household.ChangeMemberRole(memberId, MemberRole.Owner);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.DuplicateOwner);
        household.Members.Single(member => member.Id == memberId).Role.Should().Be(MemberRole.Member);
    }

    [Fact]
    public void Given_Household_When_ChangingRoleOfUnknownMember_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();

        var action = () => household.ChangeMemberRole(MemberId.New(), MemberRole.Member);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.MemberNotFound);
    }

    [Fact]
    public void Given_HouseholdWithSingleOwner_When_RemovingOwnerRoleFromOwner_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var ownerId = household.Members.Single().Id;

        var action = () => household.ChangeMemberRole(ownerId, MemberRole.Member);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotRemoveOwnerRoleFromLastOwner);
        household.Members.Single().Role.Should().Be(MemberRole.Owner);
    }

    [Fact]
    public void Given_HouseholdWithTwoOwners_When_DemotingOneOwner_Then_RoleIsReplaced()
    {
        var household = CreateValidHousehold();
        var firstOwnerId = household.Members.Single().Id;
        var secondOwnerId = MemberId.New();
        AddMemberViaReflection(household, new Member(secondOwnerId, MemberRole.Owner));

        household.ChangeMemberRole(secondOwnerId, MemberRole.Member);

        household.Members.Single(member => member.Id == secondOwnerId).Role.Should().Be(MemberRole.Member);
        household.Members.Single(member => member.Role == MemberRole.Owner).Id.Should().Be(firstOwnerId);
    }

    [Fact]
    public void Given_Household_When_ChangeMemberRole_Then_NoDomainEventIsPublished()
    {
        var household = CreateValidHousehold();
        var firstOwnerId = household.Members.Single().Id;
        var secondOwnerId = MemberId.New();
        AddMemberViaReflection(household, new Member(secondOwnerId, MemberRole.Owner));

        household.ChangeMemberRole(secondOwnerId, MemberRole.Member);

        household.Members.Single(member => member.Role == MemberRole.Owner).Id.Should().Be(firstOwnerId);
        household.DomainEvents.OfType<HouseholdCreated>().Should().HaveCount(1);
        household.DomainEvents.Should().NotContainItemsAssignableTo<HouseholdActivated>();
    }

    [Fact]
    public void Given_HouseholdWithoutOwner_When_PromotingMemberToOwner_Then_ExactlyOneOwnerIsRestored()
    {
        var household = CreateValidHousehold();
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);
        RemoveOwnerViaReflection(household);

        household.ChangeMemberRole(memberId, MemberRole.Owner);

        household.Members.Single(member => member.Id == memberId).Role.Should().Be(MemberRole.Owner);
        household.Members.Should().HaveCount(1);
    }

    [Fact]
    public void Given_ActiveHousehold_When_OwnerArchives_Then_StatusBecomesArchived()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;

        household.Archive(ownerId, OccurredAt);

        household.Status.Should().Be(HouseholdStatus.Archived);
    }

    [Fact]
    public void Given_ActiveHousehold_When_OwnerArchives_Then_ExactlyOneHouseholdArchivedEventIsPublished()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;

        household.Archive(ownerId, OccurredAt);

        household.DomainEvents.OfType<HouseholdArchived>().Should().HaveCount(1);
        var archived = household.DomainEvents.OfType<HouseholdArchived>().Single();
        archived.HouseholdId.Should().Be(household.Id);
        archived.ArchivedBy.Should().Be(ownerId);
        archived.OccurredAt.Should().Be(OccurredAt);
    }

    [Fact]
    public void Given_ActiveHousehold_When_OwnerArchives_Then_MembersAndBaseCurrencyAreUnchanged()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;

        household.Archive(ownerId, OccurredAt);

        household.Members.Should().HaveCount(1);
        household.Members.Single().Id.Should().Be(ownerId);
        household.Members.Single().Role.Should().Be(MemberRole.Owner);
        household.BaseCurrency.Should().Be(new Currency("USD"));
    }

    [Fact]
    public void Given_DraftHousehold_When_OwnerArchives_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;

        var action = () => household.Archive(ownerId, OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotArchiveExceptFromActiveState);
        household.Status.Should().Be(HouseholdStatus.Draft);
    }

    [Fact]
    public void Given_ArchivedHousehold_When_OwnerArchives_Then_DomainExceptionIsThrown()
    {
        var household = CreateArchivedHousehold();
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;

        var action = () => household.Archive(ownerId, OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.CannotArchiveExceptFromActiveState);
        household.Status.Should().Be(HouseholdStatus.Archived);
    }

    [Fact]
    public void Given_ActiveHousehold_When_NonOwnerArchives_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var memberId = MemberId.New();
        household.AddMember(memberId, MemberRole.Member);

        var action = () => household.Archive(memberId, OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.OnlyOwnerCanArchive);
        household.Status.Should().Be(HouseholdStatus.Active);
    }

    [Fact]
    public void Given_ActiveHousehold_When_UnknownMemberArchives_Then_DomainExceptionIsThrown()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);

        var action = () => household.Archive(MemberId.New(), OccurredAt);

        action.Should().Throw<DomainException>()
            .WithMessage(DomainErrors.Household.OnlyOwnerCanArchive);
        household.Status.Should().Be(HouseholdStatus.Active);
    }

    private static void AddMemberViaReflection(Household household, Member member)
    {
        var field = typeof(Household).GetField("_members", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var members = (List<Member>)field.GetValue(household)!;
        members.Add(member);
    }

    private static void RemoveOwnerViaReflection(Household household)
    {
        var field = typeof(Household).GetField("_members", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var members = (List<Member>)field.GetValue(household)!;
        members.RemoveAll(member => member.Role == MemberRole.Owner);
    }

    private static Household CreateArchivedHousehold()
    {
        var household = CreateValidHousehold();
        household.SetBaseCurrency("USD");
        household.Activate(OccurredAt);
        var ownerId = household.Members.Single(member => member.Role == MemberRole.Owner).Id;
        household.Archive(ownerId, OccurredAt);
        return household;
    }
}
