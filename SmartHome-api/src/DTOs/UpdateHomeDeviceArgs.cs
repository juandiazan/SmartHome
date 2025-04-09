namespace DTOs;

public sealed record class UpdateHomeDeviceArgs
{
    public string HardwareId { get; init; }
    public string NewAlias { get; init; }

    public UpdateHomeDeviceArgs(string hardwareId, string alias)
    {
        HardwareId = hardwareId;
        NewAlias = alias;
    }
}
