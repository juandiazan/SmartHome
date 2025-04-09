namespace Domain;
public sealed class Home
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = string.Empty;
    public string OwnerEmail { get; init; } = null!;
    public User HomeOwner { get; init; } = null!;
    public Address Address { get; init; } = null!;
    public GeographicLocation Location { get; init; } = null!;
    public int MaxAmountOfMembers { get; init; }
    public List<HomeDevice> AssociatedDevices { get; init; } = [];
    public List<Member> Members { get; init; } = [];
    public List<Room> Rooms { get; init; } = [];

    public HomeDevice AssociateDevice(HomeDevice newDevice)
    {
        newDevice.HomeId = Id;
        AssociatedDevices.Add(newDevice);
        return newDevice;
    }

    public bool HasDevice(Guid hardwareId)
    {
        return AssociatedDevices.Any(hd => hd.HardwareId == hardwareId);
    }

    public void AddMember(Member newMember)
    {
        Members.Add(newMember);
    }

    public void AddRoom(Room newRoom)
    {
        Rooms.Add(newRoom);
    }
}
