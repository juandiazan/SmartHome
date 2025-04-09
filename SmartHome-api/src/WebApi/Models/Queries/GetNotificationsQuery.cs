using Domain;
using PaginationAndFilters.Models;

namespace WebApi.Models.Queries;

public sealed class GetNotificationsQuery
{
    public DeviceType? DeviceType { get; init; }
    public DateTime? CreationDate { get; init; }
    public bool? WasRead { get; init; }

    public GetNotificationsQuery()
    {
    }

    public GetNotificationsQuery(DeviceType? deviceType, DateTime? creationDate, bool? wasRead)
    {
        DeviceType = deviceType;
        CreationDate = creationDate;
        WasRead = wasRead;
    }

    public NotificationFilterArgs ToArgs()
    {
        return new NotificationFilterArgs(DeviceType, CreationDate, WasRead);
    }
}
