namespace Domain;
public sealed class Notification
{
    public Guid Id { get; init; }

    public Guid HomeId { get; init; }
    public Home Home { get; set; } = null!;

    public Guid TriggeringDeviceId { get; init; }
    public HomeDevice TriggeringDevice { get; set; } = null!;

    public string TriggeringEvent { get; init; } = null!;

    public bool WasRead { get; set; }
    public DateTime DateTimeOfEvent { get; init; }
    public Guid UserItIsAddressedToId { get; init; }
}
