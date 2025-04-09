namespace WebApi.Models.Requests;

public sealed class AddDeviceToHomeRequest
{
    public string DeviceId { get; init; }
    public string Alias { get; init; }

    public AddDeviceToHomeRequest(string deviceId, string alias)
    {
        DeviceId = deviceId;
        Alias = alias;
    }
}
