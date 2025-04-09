using DTOs;
using PaginationAndFilters.Models;

namespace IBusinessLogic;

public interface INotificationService
{
    void GenerateAndSendNotification(NotificationGenerationArgs args);
    List<GetNotificationsOfUserArgs> GetUserNotifications(string? token, NotificationFilterArgs args);
}
