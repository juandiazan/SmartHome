namespace WebApi.Models.Responses;

public class CreateCameraResponse
{
    public string CameraName { get; init; }
    public string CameraModel { get; init; }
    public string Description { get; init; }
    public string MainPhoto { get; init; }
    public string DeviceType { get; init; }
    public bool CanBeUsedIndoors { get; init; }
    public bool CanBeUsedOutdoors { get; init; }
    public bool HasMovementDetectionSupport { get; init; }
    public bool HasPersonDetectionSupport { get; init; }

    public CreateCameraResponse(
        string cameraName,
        string cameraModel,
        string description,
        string mainPhoto,
        string deviceType,
        bool canBeUsedIndoors,
        bool canBeUsedOutdoors,
        bool hasMovementDetectionSupport,
        bool hasPersonDetectionSupport)
    {
        CameraName = cameraName;
        CameraModel = cameraModel;
        Description = description;
        MainPhoto = mainPhoto;
        DeviceType = deviceType;
        CanBeUsedIndoors = canBeUsedIndoors;
        CanBeUsedOutdoors = canBeUsedOutdoors;
        HasMovementDetectionSupport = hasMovementDetectionSupport;
        HasPersonDetectionSupport = hasPersonDetectionSupport;
    }
}
