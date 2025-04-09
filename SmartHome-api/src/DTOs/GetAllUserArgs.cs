namespace DTOs;
public sealed record class GetAllUserArgs
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Role { get; init; } = null!;
    public string CreationDate { get; init; } = null!;

    public GetAllUserArgs(string id, string name, string surname, string fullName, string role, string creationDate)
    {
        Id = id;
        Name = name;
        Surname = surname;
        FullName = fullName;
        Role = role;
        CreationDate = creationDate;
    }
}
