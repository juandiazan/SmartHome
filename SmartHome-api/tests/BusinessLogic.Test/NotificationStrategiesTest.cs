using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;
using NotificationStrategies;

namespace BusinessLogic.Test;

[TestClass]
public class NotificationStrategiesTest
{
    private Mock<INotificationRepository> _notificationRepository = null!;

    private Mock<IHomeDeviceService> _homeDeviceService = null!;
    private Mock<IHomeService> _homeService = null!;
    private Mock<IUserService> _userService = null!;
    private Mock<INotificationCreator> _notificationCreatorService = null!;

    private PersonDetectionNotificationStrategy _personNotificationStrategy = null!;
    private MovementDetectionNotificationStrategy _movementNotificationStrategy = null!;
    private WindowSensorClosedNotificationStrategy _windowSensorClosedNotificationStrategy = null!;
    private WindowSensorOpenedNotificationStrategy _windowSensorOpenedNotificationStrategy = null!;

    [TestInitialize]
    public void Initialize()
    {
        _notificationRepository = new Mock<INotificationRepository>(MockBehavior.Strict);

        _homeService = new Mock<IHomeService>(MockBehavior.Strict);
        _userService = new Mock<IUserService>(MockBehavior.Strict);
        _homeDeviceService = new Mock<IHomeDeviceService>(MockBehavior.Strict);
        _notificationCreatorService = new Mock<INotificationCreator>(MockBehavior.Strict);

        _personNotificationStrategy = new PersonDetectionNotificationStrategy(_homeService.Object, _homeDeviceService.Object, _userService.Object, _notificationCreatorService.Object);
        _movementNotificationStrategy = new MovementDetectionNotificationStrategy(_homeService.Object, _homeDeviceService.Object, _notificationCreatorService.Object);
        _windowSensorClosedNotificationStrategy = new WindowSensorClosedNotificationStrategy(_homeService.Object, _homeDeviceService.Object, _notificationCreatorService.Object);
        _windowSensorOpenedNotificationStrategy = new WindowSensorOpenedNotificationStrategy(_homeService.Object, _homeDeviceService.Object, _notificationCreatorService.Object);
    }

    #region WidonwSensorOpened
    [TestMethod]
    public void GenerateAndSendWindowSensorOpenedNotifications_WithValidParameters_ShouldSendNotification()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(returnedHomeId);

        var permission = new Permission { Name = "receive-notifications" };
        var member = new Member { Id = memberId, Permissions = [permission] };
        var members = new List<Member>() { member };

        _homeService
            .Setup(hs => hs.GetHomeMembers(returnedHomeId))
            .Returns(members);

        _notificationCreatorService
            .Setup(ncs => ncs.Create(It.IsAny<CreateNotificationArgs>()))
            .Returns(new Notification());

        _windowSensorOpenedNotificationStrategy.GenerateNotifications(hardwareId);

        _homeDeviceService.Verify();
        _homeService.Verify();
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorOpenedNotifications_WithInvalidHardwareId_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns((HomeDevice?)null);

        var act = () => _windowSensorOpenedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorOpenedNotifications_WithHomeDeviceBeingTurnedOff_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = false
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        var act = () => _windowSensorOpenedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorOpenedNotifications_WithInvalidHome_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(Guid.Empty);

        _notificationRepository
            .Setup(nr => nr.Add(It.IsAny<Notification>()))
            .Returns(new Notification());

        var act = () => _windowSensorOpenedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<KeyNotFoundException>("Home not found for the given device.");
    }
    #endregion

    #region WindowSensorClosed
    [TestMethod]
    public void GenerateAndSendWindowSensorClosedNotifications_WithValidParameters_ShouldSendNotification()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(returnedHomeId);

        var permission = new Permission { Name = "receive-notifications" };
        var member = new Member { Id = memberId, Permissions = [permission] };
        var members = new List<Member>() { member };

        _homeService
            .Setup(hs => hs.GetHomeMembers(returnedHomeId))
            .Returns(members);

        _notificationCreatorService
            .Setup(ncs => ncs.Create(It.IsAny<CreateNotificationArgs>()))
            .Returns(new Notification());

        _windowSensorClosedNotificationStrategy.GenerateNotifications(hardwareId);

        _homeDeviceService.Verify();
        _homeService.Verify();
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorClosedNotifications_WithInvalidHardwareId_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns((HomeDevice?)null);

        var act = () => _windowSensorClosedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorClosedNotifications_WithHomeDeviceBeingTurnedOff_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = false
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        var act = () => _windowSensorClosedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendWindowSensorClosedNotifications_WithInvalidHome_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(Guid.Empty);

        _notificationRepository
            .Setup(nr => nr.Add(It.IsAny<Notification>()))
            .Returns(new Notification());

        var act = () => _windowSensorClosedNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<KeyNotFoundException>("Home not found for the given device.");
    }
    #endregion

    #region MotionSensorDetectMotion
    [TestMethod]
    public void GenerateAndSendMotionDetectedNotifications_WithValidParameters_ShouldSendNotification()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(returnedHomeId);

        var permission = new Permission { Name = "receive-notifications" };
        var member = new Member { Id = memberId, Permissions = [permission] };
        var members = new List<Member>() { member };

        _homeService
            .Setup(hs => hs.GetHomeMembers(returnedHomeId))
            .Returns(members);

        _notificationCreatorService
            .Setup(ncs => ncs.Create(It.IsAny<CreateNotificationArgs>()))
            .Returns(new Notification());

        _movementNotificationStrategy.GenerateNotifications(hardwareId);

        _homeDeviceService.Verify();
        _homeService.Verify();
    }

    [TestMethod]
    public void GenerateAndSendMotionDetectedNotifications_WithInvalidHardwareId_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns((HomeDevice?)null);

        var act = () => _movementNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendMotionDetectedNotifications_WithHomeDeviceBeingTurnedOff_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = false
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        var act = () => _movementNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendMotionDetectedNotifications_WithInvalidHome_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(Guid.Empty);

        _notificationRepository
            .Setup(nr => nr.Add(It.IsAny<Notification>()))
            .Returns(new Notification());

        var act = () => _movementNotificationStrategy.GenerateNotifications(hardwareId);

        act.Should().Throw<KeyNotFoundException>("Home not found for the given device.");
    }
    #endregion

    #region CameraDetectPerson
    #endregion

    [TestMethod]
    public void GenerateAndSendCameraPersonDetectedNotifications_WithValidParameters_ShouldSendNotification()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var identifiedPerson = "person@gmail.com";

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        var identifiedUser = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "email"
        };

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(returnedHomeId);

        _userService
            .Setup(us => us.GetUserByEmail(identifiedPerson))
            .Returns(identifiedUser);

        var permission = new Permission { Name = "receive-notifications" };
        var member = new Member { Id = memberId, Permissions = [permission] };
        var members = new List<Member>() { member };

        _homeService
            .Setup(hs => hs.GetHomeMembers(returnedHomeId))
            .Returns(members);

        _notificationCreatorService
            .Setup(ncs => ncs.Create(It.IsAny<CreateNotificationArgs>()))
            .Returns(new Notification());

        _personNotificationStrategy.GenerateNotifications(hardwareId, identifiedPerson);

        _homeDeviceService.Verify();
        _homeService.Verify();
    }

    [TestMethod]
    public void GenerateAndSendCameraPersonDetectedNotifications_WithInvalidHardwareId_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var identifiedPerson = "email@gmail.com";

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns((HomeDevice?)null);

        var act = () => _personNotificationStrategy.GenerateNotifications(hardwareId, identifiedPerson);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendCameraPersonDetectedNotifications_WithHomeDeviceBeingTurnedOff_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var identifiedPerson = "email@gmail.com";

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = false
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        var act = () => _personNotificationStrategy.GenerateNotifications(hardwareId, identifiedPerson);

        act.Should().Throw<InvalidOperationException>("Device is not online or does not exist.");
    }

    [TestMethod]
    public void GenerateAndSendCameraPersonDetectedNotifications_WithInvalidHome_ShouldThrowInvalidOperationException()
    {
        var hardwareId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var identifiedPerson = "email@gmail.com";

        var returnedHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = true
        };

        var returnedHomeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(hs => hs.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(returnedHomeDevice);

        _homeService
            .Setup(hs => hs.GetHomeIdByHardwareId(hardwareId))
            .Returns(Guid.Empty);

        _notificationRepository
            .Setup(nr => nr.Add(It.IsAny<Notification>()))
            .Returns(new Notification());

        var act = () => _personNotificationStrategy.GenerateNotifications(hardwareId, identifiedPerson);

        act.Should().Throw<KeyNotFoundException>("Home not found for the given device.");
    }
}
