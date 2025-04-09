using DTOs;

namespace WebApi.Models.Requests;

public sealed class CreateCameraRequest
{
    public string? CameraName { get; init; }
    public string? CameraModel { get; init; }
    public string? Description { get; init; }
    public List<string?> Photos { get; init; }
    public string? DeviceType { get; init; }
    public bool CanBeUsedIndoors { get; init; }
    public bool CanBeUsedOutdoors { get; init; }
    public bool HasMovementDetectionSupport { get; init; }
    public bool HasPersonDetectionSupport { get; init; }

    public CreateCameraRequest(
        string cameraName,
        string cameraModel,
        string description,
        List<string?> photos,
        string deviceType,
        bool canBeUsedIndoors,
        bool canBeUsedOutdoors,
        bool hasMovementDetectionSupport,
        bool hasPersonDetectionSupport)
    {
        CameraName = cameraName;
        CameraModel = cameraModel;
        Description = description;
        Photos = photos;
        DeviceType = deviceType;
        CanBeUsedIndoors = canBeUsedIndoors;
        CanBeUsedOutdoors = canBeUsedOutdoors;
        HasMovementDetectionSupport = hasMovementDetectionSupport;
        HasPersonDetectionSupport = hasPersonDetectionSupport;
    }

    public CreateCameraArgs ToArgs()
    {
        return new CreateCameraArgs(
            CameraName ?? string.Empty,
            CameraModel ?? string.Empty,
            Description ?? string.Empty,
            Photos ?? [],
            DeviceType ?? string.Empty,
            CanBeUsedIndoors,
            CanBeUsedOutdoors,
            HasMovementDetectionSupport,
            HasPersonDetectionSupport);
    }
}
