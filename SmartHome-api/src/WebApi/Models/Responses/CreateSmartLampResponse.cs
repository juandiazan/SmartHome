namespace WebApi.Models.Responses;

public class CreateSmartLampResponse
{
    public string LampName { get; init; }
    public string LampModel { get; init; }
    public string Description { get; init; }
    public string MainPhoto { get; init; }
    public string DeviceType { get; init; }
    public bool IsTurnedOn { get; init; }

    public CreateSmartLampResponse(
        string lampName,
        string lampModel,
        string description,
        string mainPhoto,
        string deviceType,
        bool isTurnedOn)
    {
        LampName = lampName;
        LampModel = lampModel;
        Description = description;
        MainPhoto = mainPhoto;
        DeviceType = deviceType;
        IsTurnedOn = isTurnedOn;
    }
}
