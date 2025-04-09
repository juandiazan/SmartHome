namespace DTOs;
public sealed record class NotificationGenerationArgs
{
    public Guid HardwareId { get; init; }
    public string DeviceType { get; init; }
    public string Action { get; init; } = null!;
    public string? ExtraData { get; init; } = null;

    public NotificationGenerationArgs(Guid hardwareId, string deviceType, string action, string? extraData)
    {
        HardwareId = hardwareId;
        DeviceType = deviceType;
        Action = action;
        ExtraData = extraData;
    }
}
