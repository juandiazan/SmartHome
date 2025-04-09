using DTOs;
using IBusinessLogic;
using IDataAccess;
using NotificationStrategies;
using PaginationAndFilters.Models;

namespace BusinessLogic;
public class NotificationService(
    INotificationRepository notificationRepository,
    ISessionService sessionService,
    INotificationStrategyManager strategyManager) : INotificationService
{
    public void GenerateAndSendNotification(NotificationGenerationArgs args)
    {
        strategyManager.HandleNotificationGeneration(args);
    }

    public List<GetNotificationsOfUserArgs> GetUserNotifications(string? token, NotificationFilterArgs args)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException(null, "Token cannot be empty or null");
        }

        var userId = sessionService.GetUserByToken(token).Id;

        var notifications = notificationRepository.GetUserNotifications(userId, args);

        return notifications.ConvertAll(notification =>
        new GetNotificationsOfUserArgs(
            notification.TriggeringEvent,
            notification.TriggeringDevice.Device!.DeviceName,
            notification.TriggeringDevice.Device!.DeviceModel,
            notification.WasRead,
            notification.DateTimeOfEvent.ToString()));
    }
}
