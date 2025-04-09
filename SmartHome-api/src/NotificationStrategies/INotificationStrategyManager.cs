using DTOs;

namespace NotificationStrategies;
public interface INotificationStrategyManager
{
    void HandleNotificationGeneration(NotificationGenerationArgs args);
}
