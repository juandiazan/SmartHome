using DTOs;

namespace WebApi.Models.Requests;

public sealed record class ImportDevicesRequest
{
    public string? DeviceImporterId { get; init; }
    public string? FilePath { get; init; }

    public ImportDevicesRequest(string? deviceImporterId, string filePath)
    {
        DeviceImporterId = deviceImporterId;
        FilePath = filePath;
    }

    public ImportDevicesArgs ToArgs()
    {
        return new ImportDevicesArgs
        {
            DeviceImporterImplementationId = DeviceImporterId ?? string.Empty,
            Path = FilePath ?? string.Empty
        };
    }
}
