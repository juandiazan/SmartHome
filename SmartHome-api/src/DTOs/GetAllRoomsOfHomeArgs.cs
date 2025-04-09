namespace DTOs;
public sealed record class GetAllRoomsOfHomeArgs
{
    public string RoomId { get; init; }
    public string RoomName { get; init; }

    public GetAllRoomsOfHomeArgs(string roomId, string roomName)
    {
        RoomId = roomId;
        RoomName = roomName;
    }
}
