using HouseholdFinancialIntelligence.Domain.Aggregates.Household.Events;
using HouseholdFinancialIntelligence.Domain.SharedKernel;

namespace HouseholdFinancialIntelligence.Domain.Aggregates.Household;

public sealed class Household : AggregateRoot<HouseholdId>
{
    private readonly List<Member> _members = [];

    private readonly List<object> _domainEvents = [];

    private Household(
        HouseholdId id,
        HouseholdName name,
        HouseholdTimeZone timeZone,
        HouseholdLocale locale) : base(id)
    {
        Name = name;
        TimeZone = timeZone;
        Locale = locale;
        Status = HouseholdStatus.Draft;
    }

    public HouseholdName Name { get; }

    public HouseholdTimeZone TimeZone { get; }

    public HouseholdLocale Locale { get; }

    public HouseholdStatus Status { get; private set; }

    public Currency? BaseCurrency { get; private set; }

    public IReadOnlyCollection<Member> Members => _members;

    public IReadOnlyCollection<object> DomainEvents => _domainEvents;

    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static Household Create(
        string name,
        string timeZone,
        string locale,
        MemberId ownerId,
        DateTimeOffset occurredAt)
    {
        var household = new Household(
            HouseholdId.New(),
            new HouseholdName(name),
            new HouseholdTimeZone(timeZone),
            new HouseholdLocale(locale));

        household._members.Add(new Member(ownerId, MemberRole.Owner));

        household._domainEvents.Add(new HouseholdCreated(
            household.Id,
            household.Name,
            household.TimeZone,
            household.Locale,
            occurredAt));

        return household;
    }

    public void SetBaseCurrency(string currencyCode)
    {
        if (Status != HouseholdStatus.Draft)
        {
            throw new DomainException(DomainErrors.Household.BaseCurrencyCannotBeChangedOutsideDraft);
        }

        BaseCurrency = new Currency(currencyCode);
    }

    public void Activate(DateTimeOffset occurredAt)
    {
        if (Status != HouseholdStatus.Draft)
        {
            throw new DomainException(DomainErrors.Household.CannotActivateExceptFromDraftState);
        }

        if (BaseCurrency is null)
        {
            throw new DomainException(DomainErrors.Household.BaseCurrencyRequiredToActivate);
        }

        if (!HasExactlyOneOwner())
        {
            throw new DomainException(DomainErrors.Household.ExactlyOneOwnerRequiredToActivate);
        }

        Status = HouseholdStatus.Active;

        _domainEvents.Add(new HouseholdActivated(
            Id,
            BaseCurrency,
            occurredAt));
    }

    public void AddMember(MemberId memberId, MemberRole role)
    {
        if (Status == HouseholdStatus.Archived)
        {
            throw new DomainException(DomainErrors.Household.CannotJoinArchivedHousehold);
        }

        if (FindMember(memberId) is not null)
        {
            throw new DomainException(DomainErrors.Household.DuplicateMember);
        }

        if (role == MemberRole.Owner && _members.Any(member => member.Role == MemberRole.Owner))
        {
            throw new DomainException(DomainErrors.Household.DuplicateOwner);
        }

        _members.Add(new Member(memberId, role));
    }

    public void RemoveMember(MemberId memberId)
    {
        var member = FindMember(memberId);
        if (member is null)
        {
            throw new DomainException(DomainErrors.Household.MemberNotFound);
        }

        if (member.Role == MemberRole.Owner && HasExactlyOneOwner())
        {
            throw new DomainException(DomainErrors.Household.CannotRemoveLastOwner);
        }

        _members.Remove(member);
    }

    public void ChangeMemberRole(MemberId memberId, MemberRole newRole)
    {
        var member = FindMember(memberId);
        if (member is null)
        {
            throw new DomainException(DomainErrors.Household.MemberNotFound);
        }

        if (member.Role == MemberRole.Owner
            && newRole != MemberRole.Owner
            && HasExactlyOneOwner())
        {
            throw new DomainException(DomainErrors.Household.CannotRemoveOwnerRoleFromLastOwner);
        }

        if (member.Role != MemberRole.Owner
            && newRole == MemberRole.Owner
            && _members.Any(candidate => candidate.Role == MemberRole.Owner))
        {
            throw new DomainException(DomainErrors.Household.DuplicateOwner);
        }

        member.Role = newRole;
    }

    public void Archive(MemberId actedBy, DateTimeOffset occurredAt)
    {
        if (Status != HouseholdStatus.Active)
        {
            throw new DomainException(DomainErrors.Household.CannotArchiveExceptFromActiveState);
        }

        var actor = FindMember(actedBy);
        if (actor is null || actor.Role != MemberRole.Owner)
        {
            throw new DomainException(DomainErrors.Household.OnlyOwnerCanArchive);
        }

        Status = HouseholdStatus.Archived;

        _domainEvents.Add(new HouseholdArchived(
            Id,
            actedBy,
            occurredAt));
    }

    private Member? FindMember(MemberId memberId) =>
        _members.SingleOrDefault(member => member.Id == memberId);

    private bool HasExactlyOneOwner() =>
        _members.Count(member => member.Role == MemberRole.Owner) == 1;
}
