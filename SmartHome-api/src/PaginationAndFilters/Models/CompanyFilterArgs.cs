namespace PaginationAndFilters.Models;
public sealed record class CompanyFilterArgs : PaginationArgs
{
    public string? CompanyOwnerFullName { get; init; } = null;
    public string? CompanyName { get; init; } = null;

    public CompanyFilterArgs()
        : base(null, null)
    {
    }

    public CompanyFilterArgs(
        int? offset = 1,
        int? limit = 10,
        string? companyName = null,
        string? ownerName = null)
        : base(offset, limit)
    {
        CompanyName = companyName;
        CompanyOwnerFullName = ownerName;
    }
}
