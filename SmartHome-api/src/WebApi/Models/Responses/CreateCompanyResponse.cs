namespace WebApi.Models.Responses;

public class CreateCompanyResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Logotype { get; init; }
    public string Rut { get; init; }

    public CreateCompanyResponse(string id, string name, string logotype, string rut)
    {
        Id = id;
        Name = name;
        Logotype = logotype;
        Rut = rut;
    }
}
