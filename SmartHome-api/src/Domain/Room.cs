namespace Domain;
public sealed class Room
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public Guid HomeItBelongsToId { get; init; }
    public Home HomeItBelongsTo { get; init; } = null!;
    public List<HomeDevice> HomeDevices { get; init; } = [];

    public void AddHomeDevice(HomeDevice homeDevice)
    {
        HomeDevices.Add(homeDevice);
    }
}
