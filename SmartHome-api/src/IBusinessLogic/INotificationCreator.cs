using Domain;
using DTOs;

namespace IBusinessLogic;
public interface INotificationCreator
{
    Notification Create(CreateNotificationArgs args);
}
