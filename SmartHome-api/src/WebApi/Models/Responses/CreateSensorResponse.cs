namespace WebApi.Models.Responses;

public class CreateSensorResponse
{
    public string DeviceName { get; init; }
    public string DeviceModel { get; init; }
    public string Description { get; init; }
    public string MainPhoto { get; init; }
    public string DeviceType { get; init; }

    public CreateSensorResponse(
        string deviceName,
        string deviceModel,
        string description,
        string mainPhoto,
        string deviceType)
    {
        DeviceName = deviceName;
        DeviceModel = deviceModel;
        Description = description;
        MainPhoto = mainPhoto;
        DeviceType = deviceType;
    }
}
