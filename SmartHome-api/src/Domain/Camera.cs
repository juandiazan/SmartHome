namespace Domain;

public class Camera : Device
{
    public bool CanBeUsedIndoors { get; init; }
    public bool CanBeUsedOutdoors { get; init; }
    public bool HasMovementDetectionSupport { get; init; }
    public bool HasPersonDetectionSupport { get; init; }
}
