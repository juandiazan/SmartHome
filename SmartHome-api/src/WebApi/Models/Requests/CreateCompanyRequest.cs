using DTOs;

namespace WebApi.Models.Requests;
public sealed record class CreateCompanyRequest
{
    public string? CompanyName { get; init; }

    public string? Logotype { get; init; }

    public string? Rut { get; init; }
    public string? ModelValidatorId { get; init; }

    public CreateCompanyRequest(
        string companyName,
        string logotype,
        string rut,
        string modelValidatorId)
    {
        CompanyName = companyName;
        Logotype = logotype;
        Rut = rut;
        ModelValidatorId = modelValidatorId;
    }

    public CreateCompanyArgs ToArgs()
    {
        return new CreateCompanyArgs(CompanyName ?? string.Empty, Logotype ?? string.Empty, Rut ?? string.Empty, ModelValidatorId ?? string.Empty);
    }
}
