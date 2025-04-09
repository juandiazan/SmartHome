namespace DTOs;
public sealed record class GetAllCompaniesCompanyArgs
{
    public string CompanyName { get; init; }
    public string OwnerFullName { get; init; }
    public string OwnerEmail { get; init; }
    public string CompanyRut { get; init; }

    public GetAllCompaniesCompanyArgs(
        string companyName,
        string ownerFullName,
        string ownerEmail,
        string rut)
    {
        CompanyName = companyName;
        OwnerFullName = ownerFullName;
        OwnerEmail = ownerEmail;
        CompanyRut = rut;
    }
}
