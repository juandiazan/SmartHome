using Domain;
using DTOs;
using IBusinessLogic;

namespace NotificationStrategies;
public class MovementDetectionNotificationStrategy(
    IHomeService homeService,
    IHomeDeviceService homeDeviceService,
    INotificationCreator notificationService) : INotificationStrategy
{
    private const string NotificationsPermissionName = "receive-notifications";
    private const string MotionDetectionEvent = "motion-detected";

    public bool CanHandle(DeviceType deviceType, string triggeringEvent)
    {
        return (deviceType == DeviceType.MovementSensor || deviceType == DeviceType.Camera) && triggeringEvent == MotionDetectionEvent;
    }

    public void GenerateNotifications(Guid hardwareId, string? additionalData = null)
    {
        var homeDevice = homeDeviceService.GetHomeDeviceByHardwareId(hardwareId);

        if (homeDevice is null || !homeDevice.ConnectionState)
        {
            throw new InvalidOperationException("Device is not online or does not exist.");
        }

        var homeId = homeService.GetHomeIdByHardwareId(hardwareId);
        if (homeId == Guid.Empty)
        {
            throw new KeyNotFoundException("Home not found for the given device.");
        }

        var homeMembers = homeService.GetHomeMembers(homeId);
        foreach (var member in homeMembers)
        {
            if (member.Permissions.Any(p => p.Name == NotificationsPermissionName))
            {
                var args = new CreateNotificationArgs(homeId, member.Id, hardwareId, "Motion Detected");
                notificationService.Create(args);
            }
        }
    }
}
