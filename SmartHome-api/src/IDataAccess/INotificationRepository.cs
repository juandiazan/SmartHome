using Domain;
using PaginationAndFilters.Models;

namespace IDataAccess;
public interface INotificationRepository : IAddRepository<Notification>
{
    Guid GetHomeOfDevice(Guid triggeringDeviceId);
    List<Notification> GetUserNotifications(Guid userId, NotificationFilterArgs args);
}
