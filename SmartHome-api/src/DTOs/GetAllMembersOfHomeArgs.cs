namespace DTOs;

public sealed record class GetAllMembersOfHomeArgs
{
    public string Id { get; init; }
    public string MemberFullName { get; init; }
    public string MemberEmail { get; init; }
    public string MemberProfilePicture { get; init; }
    public List<string> MemberPermissions { get; init; }
    public bool CanReceiveNotifications { get; init; }

    public GetAllMembersOfHomeArgs(
        string id,
        string memberFullName,
        string memberEmail,
        string memberProfilePicture,
        List<string> memberPermissions,
        bool canReceiveNotifications)
    {
        Id = id;
        MemberFullName = memberFullName;
        MemberEmail = memberEmail;
        MemberProfilePicture = memberProfilePicture;
        MemberPermissions = memberPermissions;
        CanReceiveNotifications = canReceiveNotifications;
    }
}
