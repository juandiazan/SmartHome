using DTOs;

namespace WebApi.Models.Requests;

public sealed class CreateHomeOwnerRequest
{
    public string? ProfilePicture { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }

    public CreateHomeOwnerRequest(string profilePicture, string name, string surname, string email, string password)
    {
        ProfilePicture = profilePicture;
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }

    public CreateHomeOwnerArgs ToArgs()
    {
        return new CreateHomeOwnerArgs(Name ?? string.Empty, Surname ?? string.Empty, ProfilePicture ?? string.Empty, Email ?? string.Empty, Password ?? string.Empty);
    }
}
