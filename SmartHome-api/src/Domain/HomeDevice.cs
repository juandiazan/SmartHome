namespace Domain;
public class HomeDevice
{
    public Guid HardwareId { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = null!;
    public Guid DeviceId { get; init; }
    public Device? Device { get; init; }
    public Guid HomeId { get; set; }
    public bool ConnectionState { get; set; }
    public Guid? RoomItIsInId { get; set; }
    public Room? RoomItIsIn { get; set; }
}
