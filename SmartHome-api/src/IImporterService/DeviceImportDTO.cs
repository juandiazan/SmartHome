namespace ImporterService;
public sealed record class DeviceImportDTO
{
    public string Id { get; init; } = null!;
    public string DeviceType { get; init; } = null!;
    public string DeviceName { get; init; } = null!;
    public string DeviceModel { get; init; } = null!;
    public List<DevicePictureDTO> Photos { get; init; } = null!;
    public bool? HasPersonDetection { get; init; }
    public bool? HasMovementDetection { get; init; }
}
