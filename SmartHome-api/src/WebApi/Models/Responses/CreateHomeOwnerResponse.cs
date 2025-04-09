namespace WebApi.Models.Responses;

public class CreateHomeOwnerResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string CreationDate { get; init; }
    public string RoleName { get; init; }
    public string ProfilePicture { get; init; }

    public CreateHomeOwnerResponse(
        string id,
        string name,
        string surname,
        string email,
        string password,
        string creationDate,
        string roleName,
        string profilePicture)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreationDate = creationDate;
        RoleName = roleName;
        ProfilePicture = profilePicture;
    }
}
