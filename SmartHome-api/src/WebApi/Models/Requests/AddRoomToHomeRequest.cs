namespace WebApi.Models.Requests;

public sealed class AddRoomToHomeRequest
{
    public string RoomName { get; init; }

    public AddRoomToHomeRequest(string roomName)
    {
        RoomName = roomName ?? string.Empty;
    }
}
