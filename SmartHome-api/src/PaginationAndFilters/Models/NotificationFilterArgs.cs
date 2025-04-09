using Domain;

namespace PaginationAndFilters.Models;
public sealed record class NotificationFilterArgs
{
    public DeviceType? DeviceType { get; init; }
    public DateTime? CreationDate { get; init; }
    public bool? WasRead { get; init; }

    public NotificationFilterArgs()
    {
    }

    public NotificationFilterArgs(DeviceType? deviceType, DateTime? creationDate, bool? wasRead)
    {
        DeviceType = deviceType;
        CreationDate = creationDate;
        WasRead = wasRead;
    }
}
