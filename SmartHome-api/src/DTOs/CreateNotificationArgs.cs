namespace DTOs;
public sealed record class CreateNotificationArgs
{
    public Guid HomeId { get; init; }
    public Guid MemberId { get; init; }
    public Guid HardwareId { get; init; }
    public string TriggeringEvent { get; init; } = null!;

    public CreateNotificationArgs(Guid homeId, Guid memberId, Guid hardwareId, string triggeringEvent)
    {
        HomeId = homeId;
        MemberId = memberId;
        HardwareId = hardwareId;
        TriggeringEvent = triggeringEvent;
    }
}
