using DTOs;

namespace WebApi.Models.Requests;

public sealed class CreateCompanyOwnerRequest
{
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }

    public CreateCompanyOwnerRequest(string name, string surname, string email, string password)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }

    public CreateUserArgs ToArgs()
    {
        return new CreateUserArgs(Name ?? string.Empty, Surname ?? string.Empty, Email ?? string.Empty, Password ?? string.Empty);
    }
}
