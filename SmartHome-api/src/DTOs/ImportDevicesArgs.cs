namespace DTOs;
public sealed record class ImportDevicesArgs
{
    public string DeviceImporterImplementationId { get; init; } = null!;
    public string Path { get; init; } = null!;
}
