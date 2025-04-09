using Domain;
using DTOs;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class HomeDeviceServiceTest
{
    private Mock<IHomeDeviceRepository> homeDeviceRepository = null!;
    private HomeDeviceService homeDeviceService = null!;

    [TestInitialize]
    public void Initialize()
    {
        homeDeviceRepository = new Mock<IHomeDeviceRepository>(MockBehavior.Strict);
        homeDeviceService = new HomeDeviceService(homeDeviceRepository.Object);
    }

    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateCorrectly()
    {
        var homeId = Guid.NewGuid();
        var sensorId = Guid.NewGuid();

        var args = new CreateHomeDeviceArgs(homeId, sensorId, "alias");

        homeDeviceRepository
            .Setup(act => act.Add(It.Is<HomeDevice>(hd => hd.DeviceId == sensorId)))
            .Returns(It.Is<HomeDevice>(hd => hd.DeviceId == sensorId));

        homeDeviceRepository
            .Setup(act => act.HomeExists(h => h.Id == args.HomeId))
            .Returns(true);

        homeDeviceRepository
            .Setup(act => act.DeviceExists(d => d.Id == args.DeviceId))
            .Returns(true);

        var result = homeDeviceService.Create(args);

        result.HardwareId.Should().NotBeEmpty();
        Guid.TryParse(result.HardwareId.ToString(), out var _).Should().BeTrue();

        result.DeviceId.Should().Be(sensorId);
    }

    [TestMethod]
    public void Create_WithNonExistentDevice_ShouldThrowException()
    {
        var deviceId = Guid.NewGuid();
        var homeId = Guid.NewGuid();
        var args = new CreateHomeDeviceArgs(homeId, deviceId, "alias");

        var newHomeDevice = new HomeDevice
        {
            DeviceId = deviceId,
            ConnectionState = false
        };

        homeDeviceRepository
            .Setup(act => act.HomeExists(h => h.Id == args.HomeId))
            .Returns(true);

        homeDeviceRepository
            .Setup(act => act.DeviceExists(d => d.Id == args.DeviceId))
            .Returns(false);

        var act = () => homeDeviceService.Create(args);

        act.Should().Throw<KeyNotFoundException>("Device does not exist");
    }

    [TestMethod]
    public void Create_WithNonExistentHome_ShouldThrowException()
    {
        var deviceId = Guid.NewGuid();
        var homeId = Guid.NewGuid();

        var args = new CreateHomeDeviceArgs(homeId, deviceId, "alias");

        var newHomeDevice = new HomeDevice
        {
            HomeId = homeId,
            DeviceId = deviceId,
            ConnectionState = false
        };

        homeDeviceRepository
            .Setup(act => act.HomeExists(h => h.Id == args.HomeId))
            .Returns(false);

        var act = () => homeDeviceService.Create(args);

        act.Should().Throw<KeyNotFoundException>("Device does not exist");
    }

    [TestMethod]
    public void GetHomeDeviceByHardwareId_WithExistentHardwareId_ShouldReturnHomeDevice()
    {
        var hardwareId = Guid.NewGuid();

        var result = new HomeDevice
        {
            HardwareId = hardwareId
        };

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(result);

        var homeDevice = homeDeviceService.GetHomeDeviceByHardwareId(hardwareId);

        homeDevice.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void GetHomeDeviceByHardwareId_WithNonExistentHardwareId_ShouldThrowException()
    {
        var hardwareId = Guid.NewGuid();

        HomeDevice result = null;

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(result);

        var act = () => homeDeviceService.GetHomeDeviceByHardwareId(hardwareId);

        act.Should().Throw<KeyNotFoundException>("Home device does not exist");
    }

    [TestMethod]
    public void UpdateHomeDeviceAlias_WithIncorrectIdFormat_ShouldThrowFormatException()
    {
        var args = new UpdateHomeDeviceArgs("incorrectId", It.IsAny<string>());

        var act = () => homeDeviceService.UpdateHomeDeviceAlias(args);

        act.Should().Throw<FormatException>("Wrong device ID format");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void UpdateHomeDeviceAlias_WithNullOrEmptyAlias_ShouldThrowArgumentNullException(string alias)
    {
        var args = new UpdateHomeDeviceArgs(Guid.NewGuid().ToString(), alias);

        var act = () => homeDeviceService.UpdateHomeDeviceAlias(args);

        act.Should().Throw<ArgumentNullException>("New alias cannot be empty");
    }

    [TestMethod]
    public void UpdateHomeDeviceAlias_WithNonExistentHomeDevice_ShouldThrowKeyNotFoundException()
    {
        var args = new UpdateHomeDeviceArgs(Guid.NewGuid().ToString(), "anAlias");

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(Guid.Parse(args.HardwareId)))
            .Returns((HomeDevice?)null);

        var act = () => homeDeviceService.UpdateHomeDeviceAlias(args);

        act.Should().Throw<KeyNotFoundException>("Home device does not exist");
    }

    [TestMethod]
    public void UpdateHomeDeviceAlias_WithCorrectData_ShouldUpdate()
    {
        var hardwareId = Guid.NewGuid();

        var args = new UpdateHomeDeviceArgs(hardwareId.ToString(), "newAlias");

        var homeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            Alias = "oldAlias"
        };

        var anotherHomeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            Alias = "newAlias"
        };

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(Guid.Parse(args.HardwareId)))
            .Returns(homeDevice);

        homeDeviceRepository
            .Setup(act => act.UpdateHomeDevice(homeDevice))
            .Returns(anotherHomeDevice);

        var result = homeDeviceService.UpdateHomeDeviceAlias(args);

        result.Alias.Should().Be(anotherHomeDevice.Alias);
    }

    [TestMethod]
    public void UpdateHomeDeviceConnectionState_WithIncorrectIdFormat_ShouldThrowFormatException()
    {
        var act = () => homeDeviceService.UpdateHomeDeviceConnectionState("incorrectId");

        act.Should().Throw<FormatException>("Wrong device ID format");
    }

    [TestMethod]
    public void UpdateHomeDeviceConnectionState_WithNonExistentHardwareId_ShouldThrowKeyNotFoundException()
    {
        var hardwareId = Guid.NewGuid().ToString();

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(It.IsAny<Guid>()))
            .Returns((HomeDevice?)null);

        var act = () => homeDeviceService.UpdateHomeDeviceConnectionState(hardwareId);

        act.Should().Throw<KeyNotFoundException>("Home device does not exist");
    }

    [TestMethod]
    public void UpdateHomeDeviceConnectionState_WithCorrectData_ShouldUpdateAndReturnCurrentConnectionState()
    {
        var hardwareId = Guid.NewGuid();
        var previousConnectionState = false;

        var homeDevice = new HomeDevice
        {
            HardwareId = hardwareId,
            ConnectionState = previousConnectionState
        };

        homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(homeDevice);

        homeDeviceRepository
            .Setup(act => act.UpdateHomeDevice(homeDevice))
            .Returns(It.Is<HomeDevice>(hd => hd.HardwareId == hardwareId && hd.ConnectionState == true));

        var newConnectionState = homeDeviceService.UpdateHomeDeviceConnectionState(hardwareId.ToString());

        newConnectionState.Should().BeTrue();
    }
}
