namespace DTOs;

public sealed record class GetAllDevicesOfHomeArgs
{
    public string DeviceName { get; init; }
    public string DeviceModel { get; init; }
    public string MainPhoto { get; init; }
    public bool ConnectionState { get; init; }
    public string DeviceAlias { get; init; }
    public string HardwareId { get; init; }
    public string RoomItIsIn { get; init; }

    public GetAllDevicesOfHomeArgs(
        string deviceName,
        string deviceModel,
        string mainPhoto,
        bool connectionState,
        string deviceAlias,
        string hardwareId,
        string roomItIsIn)
    {
        DeviceName = deviceName;
        DeviceModel = deviceModel;
        MainPhoto = mainPhoto;
        ConnectionState = connectionState;
        DeviceAlias = deviceAlias;
        HardwareId = hardwareId;
        RoomItIsIn = roomItIsIn;
    }
}
