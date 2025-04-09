namespace DTOs;

public sealed record class GetNotificationsOfUserArgs
{
    public string TriggeringEvent { get; }
    public string TriggeringDeviceName { get; }
    public string TriggeringDeviceModel { get; }
    public bool WasRead { get; }
    public string DateTimeOcurred { get; }

    public GetNotificationsOfUserArgs(
        string triggeringEvent,
        string triggeringDeviceName,
        string triggeringDeviceModel,
        bool wasRead,
        string dateTimeOcurred)
    {
        TriggeringEvent = triggeringEvent;
        TriggeringDeviceName = triggeringDeviceName;
        TriggeringDeviceModel = triggeringDeviceModel;
        WasRead = wasRead;
        DateTimeOcurred = dateTimeOcurred;
    }
}
