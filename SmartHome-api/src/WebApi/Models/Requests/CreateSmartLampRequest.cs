using DTOs;

namespace WebApi.Models.Requests;

public sealed class CreateSmartLampRequest
{
    public string? LampName { get; init; }
    public string? LampModel { get; init; }
    public string? Description { get; init; }
    public List<string?> Photos { get; init; }
    public string? DeviceType { get; init; }
    public bool IsTurnedOn { get; init; }

    public CreateSmartLampRequest(
        string lampName,
        string lampModel,
        string description,
        List<string?> photos,
        string deviceType)
    {
        LampName = lampName;
        LampModel = lampModel;
        Description = description;
        Photos = photos;
        DeviceType = deviceType;
        IsTurnedOn = false;
    }

    public CreateSmartLampArgs ToArgs()
    {
        return new CreateSmartLampArgs(
            LampName ?? string.Empty,
            LampModel ?? string.Empty,
            Description ?? string.Empty,
            Photos ?? [],
            IsTurnedOn,
            DeviceType ?? string.Empty);
    }
}
