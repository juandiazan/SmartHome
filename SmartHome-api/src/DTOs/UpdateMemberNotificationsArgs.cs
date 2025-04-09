namespace DTOs;

public sealed record class UpdateMemberNotificationsArgs
{
    public bool NotificationsEnabled { get; init; }
    public Guid MemberId { get; init; }

    public UpdateMemberNotificationsArgs(bool notificationsEnabled, Guid memberId)
    {
        NotificationsEnabled = notificationsEnabled;
        MemberId = memberId;
    }
}
