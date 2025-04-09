namespace JsonImporter;

public sealed record class DevicePictureJsonDTO
{
    public string? Path { get; init; }
    public bool? EsPrincipal { get; init; }
}
