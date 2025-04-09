using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;
using NotificationStrategies;
using PaginationAndFilters.Models;

namespace BusinessLogic.Test;

[TestClass]
public class NotificationServiceTest
{
    private Mock<INotificationRepository> _notificationRepository = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<INotificationStrategyManager> _strategyManager = null!;
    private NotificationService _notificationService = null!;

    private NotificationCreator _notificationCreator = null!;

    [TestInitialize]
    public void Initialize()
    {
        _notificationRepository = new Mock<INotificationRepository>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _strategyManager = new Mock<INotificationStrategyManager>(MockBehavior.Strict);

        _notificationService =
            new NotificationService(
            _notificationRepository.Object,
            _sessionService.Object,
            _strategyManager.Object);

        _notificationCreator = new NotificationCreator(_notificationRepository.Object);
    }

    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateCorrectly()
    {
        var triggeringDeviceId = Guid.NewGuid();
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var action = "action";
        var date = DateTime.Today;
        var notifId = Guid.NewGuid();

        var newNotificationArgs = new CreateNotificationArgs(homeId, memberId, triggeringDeviceId, action);

        var expectedNotification = new Notification
        {
            Id = notifId,
            HomeId = homeId,
            WasRead = false,
            DateTimeOfEvent = date,
            TriggeringDeviceId = triggeringDeviceId,
            TriggeringEvent = action,
            UserItIsAddressedToId = memberId
        };

        _notificationRepository
            .Setup(nr => nr.Add(It.IsAny<Notification>()))
            .Returns(expectedNotification);

        var result = _notificationCreator.Create(newNotificationArgs);

        result.TriggeringEvent.Should().Be(expectedNotification.TriggeringEvent);
        result.TriggeringDeviceId.Should().Be(expectedNotification.TriggeringDeviceId);
        result.HomeId.Should().Be(expectedNotification.HomeId);
        result.DateTimeOfEvent.Date.Should().Be(expectedNotification.DateTimeOfEvent.Date);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptyTriggeringEvent_ShouldThrowException(string triggeringEvent)
    {
        var triggeringDeviceId = Guid.NewGuid();
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var newNotificationArgs = new CreateNotificationArgs(homeId, memberId, triggeringDeviceId, triggeringEvent);

        var act = () => _notificationCreator.Create(newNotificationArgs);

        act.Should().Throw<ArgumentNullException>("Triggering event cannot be empty or null");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void GetUserNotifications_WithNullOrEmptyToken_ShouldThrowException(string token)
    {
        var act = () => _notificationService.GetUserNotifications(token, new NotificationFilterArgs());

        act.Should().Throw<ArgumentNullException>("Token cannot be empty or null");
    }

    [TestMethod]
    public void GetUserNotifications_WithValidToken_ShouldReturnNotifications()
    {
        var token = "validToken";
        var userId = Guid.NewGuid();
        var returnedNotif = new Notification
        {
            Id = Guid.NewGuid(),
            HomeId = Guid.NewGuid(),
            WasRead = false,
            DateTimeOfEvent = DateTime.Now,
            TriggeringDeviceId = Guid.NewGuid(),
            TriggeringEvent = "Window opened",
            TriggeringDevice = new HomeDevice
            {
                Device = new Device
                {
                    DeviceName = "Name",
                    DeviceModel = "Model"
                }
            }
        };

        var notificationArgs = new List<GetNotificationsOfUserArgs>
        {
            new(returnedNotif.TriggeringEvent, "Name", "Model", false, returnedNotif.DateTimeOfEvent.ToString())
        };

        _sessionService
            .Setup(service => service.GetUserByToken(token))
            .Returns(new User { Id = userId });

        _notificationRepository
            .Setup(repo => repo.GetUserNotifications(userId, new NotificationFilterArgs()))
            .Returns([returnedNotif]);

        var result = _notificationService.GetUserNotifications(token, new NotificationFilterArgs());

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(notificationArgs);
    }

    [TestMethod]
    public void HandleNotificationGeneration_WithCorrectData_ShouldGenerate()
    {
        var hardwareId = Guid.NewGuid();
        var deviceType = DeviceType.Sensor.ToString();
        var triggeringEvent = "event";

        var args = new NotificationGenerationArgs(hardwareId, deviceType, triggeringEvent, null);
        _strategyManager
            .Setup(sm => sm.HandleNotificationGeneration(args))
            .Verifiable();

        _notificationService.GenerateAndSendNotification(args);

        _strategyManager.Verify();
    }

    [TestMethod]
    public void HandleNotificationGeneration_WithNoStrategies_ShouldThrowKeyNotFoundException()
    {
        var hardwareId = Guid.NewGuid();
        var deviceType = "Sensor";
        var triggeringEvent = "event";
        var args = new NotificationGenerationArgs(hardwareId, deviceType, triggeringEvent, null);

        _strategyManager
            .Setup(sm => sm.HandleNotificationGeneration(args))
            .Throws(new KeyNotFoundException($"No notification strategy found for device type {args.DeviceType}"));

        var act = () => _notificationService.GenerateAndSendNotification(args);

        act.Should().Throw<KeyNotFoundException>($"No notification strategy found for device type {args.DeviceType}");
    }
}
