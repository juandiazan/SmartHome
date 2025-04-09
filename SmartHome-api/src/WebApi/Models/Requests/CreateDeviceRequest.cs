using DTOs;

namespace WebApi.Models.Requests;

public sealed class CreateDeviceRequest
{
    public string? DeviceName { get; init; }
    public string? DeviceModel { get; init; }
    public string? Description { get; init; }
    public List<string?> Photos { get; init; }
    public string? DeviceType { get; init; }

    public CreateDeviceRequest(string deviceName, string deviceModel, string description, List<string?> photos, string deviceType)
    {
        DeviceName = deviceName;
        DeviceModel = deviceModel;
        Description = description;
        Photos = photos;
        DeviceType = deviceType;
    }

    public CreateDeviceArgs ToArgs()
    {
        return new CreateDeviceArgs(DeviceName!, DeviceModel!, Description!, Photos!, DeviceType!);
    }
}
