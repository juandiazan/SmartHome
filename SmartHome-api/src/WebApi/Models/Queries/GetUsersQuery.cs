using PaginationAndFilters.Models;

namespace WebApi.Models.Queries;

public sealed class GetUsersQuery
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }

    public GetUsersQuery()
    {
    }

    public GetUsersQuery(int? offset, int? limit, string? name, string? role)
    {
        Offset = offset;
        Limit = limit;
        Name = name;
        Role = role;
    }

    public UserFilterArgs ToArgs()
    {
        return new UserFilterArgs(Offset, Limit, Name, Role);
    }
}
