using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public sealed class NotificationCreator(INotificationRepository notificationRepository) : INotificationCreator
{
    public Notification Create(CreateNotificationArgs args)
    {
        if (string.IsNullOrEmpty(args.TriggeringEvent))
        {
            throw new ArgumentNullException(null, "Triggering event cannot be empty or null");
        }

        var newNotification = new Notification
        {
            Id = Guid.NewGuid(),
            HomeId = args.HomeId,
            TriggeringDeviceId = args.HardwareId,
            TriggeringEvent = args.TriggeringEvent,
            WasRead = false,
            DateTimeOfEvent = DateTime.Now,
            UserItIsAddressedToId = args.MemberId
        };

        notificationRepository.Add(newNotification);

        return newNotification;
    }
}
