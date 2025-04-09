using Domain;
using DTOs;

namespace NotificationStrategies;
public sealed class NotificationStrategyManager : INotificationStrategyManager
{
    private readonly IEnumerable<INotificationStrategy> _strategies;

    public NotificationStrategyManager(IEnumerable<INotificationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public void HandleNotificationGeneration(NotificationGenerationArgs args)
    {
        var strategy = ChooseStrategy(Enum.Parse<DeviceType>(args.DeviceType), args.Action);
        if (strategy is not null)
        {
            strategy.GenerateNotifications(args.HardwareId, args.ExtraData);
        }
        else
        {
            throw new KeyNotFoundException($"No notification strategy found for device type {args.DeviceType}");
        }
    }

    private INotificationStrategy? ChooseStrategy(DeviceType deviceType, string triggeringEvent)
    {
        return _strategies.FirstOrDefault(strategy => strategy.CanHandle(deviceType, triggeringEvent));
    }
}
