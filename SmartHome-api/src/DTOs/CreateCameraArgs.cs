namespace DTOs;

public record class CreateCameraArgs : CreateDeviceArgs
{
    public bool CanBeUsedIndoors { get; init; }
    public bool CanBeUsedOutdoors { get; init; }
    public bool HasMovementDetectionSupport { get; init; }
    public bool HasPersonDetectionSupport { get; init; }

    public CreateCameraArgs(
        string cameraName,
        string cameraModel,
        string description,
        List<string> photos,
        string deviceType,
        bool canBeUsedIndoors,
        bool canBeUsedOutdoors,
        bool hasMovementDetectionSupport,
        bool hasPersonDetectionSupport)
        : base(cameraName, cameraModel, description, photos, deviceType)
    {
        CanBeUsedIndoors = canBeUsedIndoors;
        CanBeUsedOutdoors = canBeUsedOutdoors;
        HasMovementDetectionSupport = hasMovementDetectionSupport;
        HasPersonDetectionSupport = hasPersonDetectionSupport;
    }
}
