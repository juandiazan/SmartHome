namespace DTOs;

public sealed record class GetAllImplementationsArgs
{
    public string Id { get; init; }
    public string Name { get; init; }

    public GetAllImplementationsArgs(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
