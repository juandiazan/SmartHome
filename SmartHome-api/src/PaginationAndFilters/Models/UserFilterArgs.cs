namespace PaginationAndFilters.Models;
public sealed record class UserFilterArgs : PaginationArgs
{
    public string? Name { get; init; } = null;
    public string? Role { get; init; } = null;
    public UserFilterArgs()
        : base(null, null)
    {
    }

    public UserFilterArgs(
        int? offset = 1,
        int? limit = 10,
        string? name = null,
        string? role = null)
        : base(offset, limit)
    {
        Name = name;
        Role = role;
    }
}
