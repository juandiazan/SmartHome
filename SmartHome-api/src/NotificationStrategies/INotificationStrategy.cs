using Domain;

namespace NotificationStrategies;

public interface INotificationStrategy
{
    bool CanHandle(DeviceType deviceType, string triggeringEvent);
    void GenerateNotifications(Guid hardwareId, string? additionalData = null);
}
