namespace DTOs;
public sealed record class GetAllDevicesArgs
{
    public string Id { get; init; }
    public string DeviceName { get; init; }
    public string DeviceModel { get; init; }
    public string MainPhoto { get; init; }
    public string OwnerCompanyName { get; init; }
    public string DeviceType { get; init; }
    public GetAllDevicesArgs(
        string id,
        string deviceName,
        string deviceModel,
        string mainPhoto,
        string ownerCompanyName,
        string deviceType)
    {
        Id = id;
        DeviceName = deviceName;
        DeviceModel = deviceModel;
        MainPhoto = mainPhoto;
        OwnerCompanyName = ownerCompanyName;
        DeviceType = deviceType;
    }
}
