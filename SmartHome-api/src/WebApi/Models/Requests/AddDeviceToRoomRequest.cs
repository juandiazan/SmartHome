namespace WebApi.Models.Requests;

public sealed class AddDeviceToRoomRequest
{
    public string HardwareId { get; init; }

    public AddDeviceToRoomRequest(string hardwareId)
    {
        HardwareId = hardwareId;
    }
}
