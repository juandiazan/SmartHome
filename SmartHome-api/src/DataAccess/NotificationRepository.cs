using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace DataAccess;
public class NotificationRepository(SmartHomeDBContext context) : INotificationRepository
{
    private readonly SmartHomeDBContext _context = context;

    public Notification Add(Notification newNotification)
    {
        var newNotificationHome = _context.Set<Home>().FirstOrDefault(home => home.Id == newNotification.HomeId);
        var newNotificationDevice = _context.Set<HomeDevice>().FirstOrDefault(homedevice => homedevice.HardwareId == newNotification.TriggeringDeviceId);

        newNotification.Home = newNotificationHome!;
        newNotification.TriggeringDevice = newNotificationDevice!;

        _context.Notifications.Add(newNotification);
        _context.SaveChanges();

        return newNotification;
    }

    public Guid GetHomeOfDevice(Guid triggeringDeviceId)
    {
        return _context.Set<HomeDevice>().FirstOrDefault(device => device.HardwareId == triggeringDeviceId)!.HomeId;
    }

    public List<Notification> GetUserNotifications(Guid userId, NotificationFilterArgs args)
    {
        var notifications =
            _context.Notifications
            .Include(n => n.TriggeringDevice).ThenInclude(hd => hd.Device)
            .Include(n => n.Home).ThenInclude(h => h.Members)
            .Where(notification => notification.Home.Members.Any(m => m.AssociatedHomeOwnerId == userId && m.Id == notification.UserItIsAddressedToId));

        var filteredNotifs = PaginationFilterService.FilterNotifications(notifications, args);

        return filteredNotifs;
    }
}
