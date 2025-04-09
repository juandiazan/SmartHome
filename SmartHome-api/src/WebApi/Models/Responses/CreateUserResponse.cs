namespace WebApi.Models.Responses;

public class CreateUserResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string CreationDate { get; init; }
    public string RoleName { get; init; }

    public CreateUserResponse(string id, string name, string surname, string email, string password, string creationDate, string roleName)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreationDate = creationDate;
        RoleName = roleName;
    }
}
