namespace DTOs;
public sealed record class CreateHomeDeviceArgs
{
    public Guid HomeId { get; init; }
    public Guid DeviceId { get; init; }
    public string HomeDeviceAlias { get; init; } = null!;
    public CreateHomeDeviceArgs(Guid homeId, Guid deviceId, string homeDeviceAlias)
    {
        HomeId = homeId;
        DeviceId = deviceId;
        HomeDeviceAlias = homeDeviceAlias;
    }
}
