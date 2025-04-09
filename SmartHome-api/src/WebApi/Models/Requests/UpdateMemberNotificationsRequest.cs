using DTOs;

namespace WebApi.Models.Requests;

public sealed class UpdateMemberNotificationsRequest
{
    public bool? NotificationsEnabled { get; init; }
    public Guid MemberId { get; init; }

    public UpdateMemberNotificationsRequest(bool? notificationsEnabled, Guid memberId)
    {
        NotificationsEnabled = notificationsEnabled;
        MemberId = memberId;
    }

    public UpdateMemberNotificationsArgs ToArgs()
    {
        return new UpdateMemberNotificationsArgs(NotificationsEnabled ?? false, MemberId);
    }
}
