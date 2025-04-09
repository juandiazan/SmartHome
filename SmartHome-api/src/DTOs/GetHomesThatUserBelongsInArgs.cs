namespace DTOs;
public sealed record class GetHomesThatUserBelongsInArgs
{
    public string HomeId { get; init; }
    public string HomeAlias { get; init; }
    public List<string> Permissions { get; init; }
    public bool IsOwner { get; init; }

    public GetHomesThatUserBelongsInArgs(
        string homeId,
        string homeAlias,
        List<string> permissions,
        bool isOwner)
    {
        HomeId = homeId;
        HomeAlias = homeAlias;
        Permissions = permissions;
        IsOwner = isOwner;
    }
}
