using PaginationAndFilters.Models;

namespace WebApi.Models.Queries;

public sealed class GetCompaniesQuery
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyOwnerFullName { get; set; }

    public GetCompaniesQuery()
    {
    }

    public GetCompaniesQuery(int? offset, int? limit, string? companyName, string? companyOwnerFullName)
    {
        Offset = offset;
        Limit = limit;
        CompanyName = companyName;
        CompanyOwnerFullName = companyOwnerFullName;
    }

    public CompanyFilterArgs ToArgs()
    {
        return new CompanyFilterArgs(Offset, Limit, CompanyName, CompanyOwnerFullName);
    }
}
