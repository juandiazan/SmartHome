using System.Data;
using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class HomeServiceTest
{
    private Mock<IHomeRepository> _homeRepository = null!;
    private Mock<IHomeDeviceRepository> _homeDeviceRepository = null!;
    private Mock<ISessionService> _sessionService = null!;
    private HomeService _homeService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeRepository = new Mock<IHomeRepository>(MockBehavior.Strict);
        _homeDeviceRepository = new Mock<IHomeDeviceRepository>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _homeService = new HomeService(_homeRepository.Object, _homeDeviceRepository.Object, _sessionService.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithEmptyOrNullMainSt_ShouldThrowHomeServiceException(string mainSt)
    {
        var act = () => new CreateHomeArgs(
            "Email",
            mainSt,
            1111,
            "50.0435436",
            "41.5437895",
            5,
            "alias");

        act.Should().Throw<ArgumentNullException>().WithMessage("Main street cannot be null or empty");
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Create_WithZeroOrNegativeDoorNumber_ShouldThrowHomeServiceException(int doorNumber)
    {
        var act = () => new CreateHomeArgs(
            "Email",
            "MainSt",
            doorNumber,
            "50.0435436",
            "41.5437895",
            5,
            "alias");

        act.Should().Throw<FormatException>().WithMessage("Door number cannot be less than zero");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptyLatitude_ShouldThrowHomeServiceException(string latitude)
    {
        var act = () => new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            latitude,
            "41.5437895",
            5,
            "alias");

        act.Should().Throw<ArgumentNullException>().WithMessage("Latitude cannot be null or empty");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptyLongitude_ShouldThrowHomeServiceException(string longitude)
    {
        var act = () => new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            "50.0435436",
            longitude,
            5,
            "alias");

        act.Should().Throw<ArgumentNullException>().WithMessage("Longitude cannot be null or empty");
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(1)]
    public void Create_WithInvalidMaxAmountOfMembers_ShouldThrowHomeServiceException(int maxMembers)
    {
        var act = () => new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            "50.0435436",
            "41.5437895",
            maxMembers,
            "alias");

        act.Should().Throw<FormatException>().WithMessage("Maximum amount of members cannot be less than zero");
    }
    #endregion

    #region Success
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateCorrectly()
    {
        var newHome = new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            "50.0435436",
            "41.5437895",
            5,
            "alias");

        var homeOwner = new HomeOwner { Email = "Email" };

        _homeRepository
            .Setup(repo => repo.GetHomeOwnerByEmail(It.IsAny<string>()))
            .Returns(homeOwner);

        _homeRepository
            .Setup(repo => repo.Add(It.IsAny<Home>()))
            .Returns(new Home());

        _homeRepository
            .Setup(act => act.Add(It.Is<Home>(h =>
                h.HomeOwner.Email == newHome.OwnerEmail &&
                h.Address.MainStreet == newHome.MainStreet &&
                h.Address.DoorNumber == newHome.DoorNumber &&
                h.Location.Latitude == newHome.Latitude &&
                h.Location.Longitude == newHome.Longitude &&
                h.MaxAmountOfMembers == newHome.MaxAmountOfMembers)))
            .Returns(new Home());

        _homeRepository
            .Setup(act => act.GetAddDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetListDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetChangeAliasOfDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetReceiveNotificationsPermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.AddMemberToHome(It.IsAny<Guid>(), It.IsAny<Member>()))
            .Returns(It.IsAny<Member>());

        var result = _homeService.Create(newHome);

        result.Id.Should().NotBeEmpty();
        Guid.TryParse(result.Id.ToString(), out var _).Should().BeTrue();

        result.HomeOwner.Email.Should().Be(newHome.OwnerEmail);
        result.MaxAmountOfMembers.Should().Be(newHome.MaxAmountOfMembers);
    }
    #endregion
    #endregion

    [TestMethod]
    public void AssociateDevice_WithCorrectData_ShouldAssociateCorrectly()
    {
        var homeDeviceRepository = new Mock<IHomeDeviceRepository>(MockBehavior.Strict);

        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var homeDevice = new HomeDevice
        {
            DeviceId = deviceId,
            HomeId = homeId,
            HardwareId = hardwareId
        };

        _homeRepository
            .Setup(act => act.GetHomeIdByHardwareId(hardwareId))
            .Returns(homeId);

        _homeRepository
            .Setup(act => act.AssociateDevice(homeId, hardwareId))
            .Returns(homeDevice);

        var result = _homeService.AssociateDevice(homeId, hardwareId);

        result.Should().BeEquivalentTo(homeDevice);
    }

    [TestMethod]
    public void AssociateDevice_WithEmptyDeviceId_ShouldThrowException()
    {
        var nonExistentHardwareId = Guid.Empty;
        var homeId = Guid.Empty;

        _homeRepository
            .Setup(act => act.GetHomeIdByHardwareId(nonExistentHardwareId))
            .Returns(homeId);

        _homeRepository
            .Setup(repo => repo.AssociateDevice(homeId, nonExistentHardwareId));

        var act = () => _homeService.AssociateDevice(homeId, nonExistentHardwareId);

        act.Should().Throw<ArgumentNullException>("Hardware Id cannot be empty");
    }

    [TestMethod]
    public void AssociateDevice_WithNonExistentDeviceId_ShouldThrowException()
    {
        var nonExistentHardwareId = Guid.NewGuid();

        _homeRepository
            .Setup(act => act.GetHomeIdByHardwareId(nonExistentHardwareId))
            .Returns(Guid.Empty);

        var act = () => _homeService.AssociateDevice(It.IsAny<Guid>(), nonExistentHardwareId);

        act.Should().Throw<KeyNotFoundException>("Device does not exist");
    }

    [TestMethod]
    public void AddMember_WithCorrectData_ShouldAddCorrectlyToHome()
    {
        var memberPermissionCanAddDeviceToHome = false;
        var memberPermissionCanSeeDevicesOfHome = false;
        var memberPermissionCanChangeAliasOfDevicesOfHome = false;
        var homeToBeAddedToId = Guid.NewGuid();

        var member = new Member
        {
            Id = Guid.NewGuid(),
            Permissions = []
        };
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
        };

        var home = new Home
        {
            Id = homeToBeAddedToId,
            OwnerEmail = "owner@domain.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Members = [],
            HomeOwner = homeOwner
        };

        _homeRepository
            .Setup(act => act.Add(It.IsAny<Home>()))
            .Returns(home);

        _homeRepository
            .Setup(act => act.AddMemberToHome(homeToBeAddedToId, It.IsAny<Member>()))
            .Returns(member);

        _homeRepository
            .Setup(act => act.HomeExists(homeToBeAddedToId))
            .Returns(true);
        _homeRepository
            .Setup(act => act.GetHomeOwnerByEmail("NewMemberEmail@domain.com"))
            .Returns(homeOwner);

        _homeRepository.Object.Add(home);

        var newMemberToAddArgs = new AddMemberToHomeArgs(
            "NewMemberEmail@domain.com",
            memberPermissionCanAddDeviceToHome,
            memberPermissionCanSeeDevicesOfHome,
            memberPermissionCanChangeAliasOfDevicesOfHome);

        var homeResult = _homeService.AddMemberToHome(homeToBeAddedToId, newMemberToAddArgs);

        homeResult.Should().Be(member);
    }

    [TestMethod]
    public void ListHomeDevices_WithCorrectData_ShouldListCorrectly()
    {
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var deviceAias = "alias";

        var homeDeviceRepository = new Mock<IHomeDeviceRepository>(MockBehavior.Strict);
        var homeDeviceService = new HomeDeviceService(homeDeviceRepository.Object);
        var args = new CreateHomeDeviceArgs(homeId, deviceId, "alias");

        var newDevice = new Device
        {
            Id = deviceId,
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor
        };

        var getAllDevicesArgsResult = new GetAllDevicesOfHomeArgs(
            newDevice.DeviceName,
            newDevice.DeviceModel,
            newDevice.Photos[0],
            false,
            deviceAias,
            hardwareId.ToString(),
            "None");

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(act => act.ListHomeDevices(homeId, null))
            .Returns([new HomeDevice { HardwareId = hardwareId, DeviceId = deviceId, Device = newDevice, Alias = "alias" }]);

        var homeDevices = _homeService.ListHomeDevices(homeId, null);

        homeDevices.Should().NotBeEmpty();
        homeDevices.Should().HaveCount(1);
        homeDevices.Should().Contain(getAllDevicesArgsResult);
    }

    [TestMethod]
    public void ListHomeDevices_EmptyGuid_ShouldThrowException()
    {
        var homeId = Guid.Empty;

        _homeRepository
            .Setup(act => act.ListHomeDevices(homeId, null))
            .Returns([]);

        var act = () => _homeService.ListHomeDevices(homeId, null);

        act.Should().Throw<FormatException>("Home Id cannot be empty");
    }

    [TestMethod]
    public void ListHomeDevices_WithNonExistentHomeId_ShouldThrowException()
    {
        var homeId = Guid.NewGuid();

        _homeRepository
            .Setup(act => act.ListHomeDevices(homeId, null))
            .Returns([]);

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(false);

        var act = () => _homeService.ListHomeDevices(homeId, null);

        act.Should().Throw<FormatException>("Home Id does not exist");
    }

    [TestMethod]
    public void AddMember_WithIncorrectEmail_ShouldThrowArgumentException()
    {
        var memberPermissionCanAddDeviceToHome = false;
        var memberPermissionCanSeeDevicesOfHome = false;
        var memberPermissionCanChangeAliasOfDevicesOfHome = false;
        var homeToBeAddedToId = Guid.NewGuid();

        var member = new Member
        {
            Id = Guid.NewGuid(),
            Permissions = []
        };

        var home = new Home
        {
            Id = homeToBeAddedToId,
            OwnerEmail = "owner@domain.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Members = []
        };

        _homeRepository
            .Setup(act => act.Add(It.IsAny<Home>()))
            .Returns(home);

        _homeRepository
            .Setup(act => act.AddMemberToHome(homeToBeAddedToId, It.IsAny<Member>()))
            .Returns(member);

        _homeRepository.Object.Add(home); // Add the home to the repository

        var newMemberToAddArgs = new AddMemberToHomeArgs(
            "Invalid Email",
            memberPermissionCanAddDeviceToHome,
            memberPermissionCanSeeDevicesOfHome,
            memberPermissionCanChangeAliasOfDevicesOfHome);

        var act = () => _homeService.AddMemberToHome(homeToBeAddedToId, newMemberToAddArgs);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format");
    }

    [TestMethod]
    public void AddMember_WithNonExistingHome_ShouldThrowException()
    {
        var nonExistingHomeId = Guid.NewGuid();

        var newMemberToAddArgs = new AddMemberToHomeArgs(
            "newmember@domain.com",
            false,
            false,
            false);

        _homeRepository
            .Setup(act => act.HomeExists(nonExistingHomeId))
            .Returns(false);

        var act = () => _homeService.AddMemberToHome(nonExistingHomeId, newMemberToAddArgs);

        act.Should().Throw<KeyNotFoundException>().WithMessage("Home not found");
    }

    [TestMethod]
    public void ListMembers_WithCorrectData_ShouldListCorrectly()
    {
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var permission = new Permission { Name = "receive-notifications" };
        var homeOwner = new HomeOwner { Email = "Email", Name = "Full", Surname = "Name", ProfilePicture = "ProfilePicture" };
        var member = new Member
        {
            Id = memberId,
            AssociatedHomeOwner = homeOwner,
            Permissions = [permission]
        };

        var args = new GetAllMembersOfHomeArgs(memberId.ToString(), "Full Name", "Email", "ProfilePicture", ["receive-notifications"], true);

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(true);

        _homeRepository
             .Setup(act => act.ListMembersOfHome(homeId))
             .Returns([member]);

        _homeRepository
            .Setup(hr => hr.GetReceiveNotificationsPermission())
            .Returns(permission);

        var members = _homeService.ListMembersOfHome(homeId);

        members.Should().NotBeEmpty();
        members.Should().HaveCount(1);
        members[0].Should().BeEquivalentTo(args);
    }

    [TestMethod]
    public void ListMembers_WithNonExistentHome_ShouldThrowException()
    {
        var homeId = Guid.NewGuid();

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(false);

        var act = () => _homeService.ListMembersOfHome(homeId);

        act.Should().Throw<KeyNotFoundException>().WithMessage("Home Id does not exist");
    }

    [TestMethod]
    public void UpdateMemberNotifications_WithExistingHome_ShouldUpdateNotifications()
    {
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var args = new UpdateMemberNotificationsArgs(true, memberId);
        var permission = new Permission { Name = "receive-notifications" };
        var member = new Member { Id = memberId, Permissions = [permission] };
        var home = new Home { Id = homeId, Members = [member] };

        _homeRepository
            .Setup(repo => repo.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(hs => hs.GetHomeById(homeId))
            .Returns(home);

        _homeRepository
            .Setup(hr => hr.GetReceiveNotificationsPermission())
            .Returns(permission);

        _homeRepository
            .Setup(repo => repo.UpdateMemberNotifications(member))
            .Verifiable();

        _homeService.UpdateMemberNotifications(homeId, args);

        _homeRepository.Verify();
    }

    [TestMethod]
    public void UpdateMemberNotifications_WithNonExistentHome_ShouldThrowKeyNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var args = new UpdateMemberNotificationsArgs(true, memberId);

        _homeRepository
            .Setup(repo => repo.HomeExists(homeId))
            .Returns(false);

        var act = () => _homeService.UpdateMemberNotifications(homeId, args);

        act.Should().Throw<KeyNotFoundException>().WithMessage("Home Id does not exist");
    }

    [TestMethod]
    public void UpdateMemberNotifications_WithNonExistentMember_ShouldThrowKeyNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var home = new Home { Id = homeId, Members = [] };

        var args = new UpdateMemberNotificationsArgs(true, memberId);

        _homeRepository
            .Setup(repo => repo.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(hr => hr.GetHomeById(homeId))
            .Returns(home);

        var act = () => _homeService.UpdateMemberNotifications(homeId, args);

        act.Should().Throw<KeyNotFoundException>().WithMessage("Member does not exist");
    }

    [TestMethod]
    public void GetHomeMember_WhenMemberExists_ShouldReturnMember()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var member = new Member { Id = memberId };

        _homeRepository.Setup(repo => repo.HomeExists(homeId)).Returns(true);
        _homeRepository.Setup(repo => repo.GetHomeMembers(homeId)).Returns([member]);

        // Act
        var result = _homeService.GetHomeMembers(homeId);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result.First().Should().Be(member);
    }

    [TestMethod]
    public void GetHomeMember_WhenHomeDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var member = new Member { Id = memberId };

        _homeRepository.Setup(repo => repo.HomeExists(homeId)).Returns(false);
        _homeRepository.Setup(repo => repo.GetHomeMembers(homeId)).Returns([member]);

        // Act
        var act = () => _homeService.GetHomeMembers(homeId);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home Id does not exist");
    }

    [TestMethod]
    public void AddRoomToHome_WithCorrectData_ShouldAddCorrectlyToHome()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var roomName = "new room";

        var newRoom = new Room
        {
            Id = roomId,
            Name = roomName
        };

        var newHome = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = []
        };

        var newHomeWithRoom = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = [newRoom]
        };

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(act => act.GetHomeById(homeId))
            .Returns(newHome);

        _homeRepository
            .Setup(act => act.UpdateHome(newHome, It.Is<Room>(r => r.Name == newRoom.Name && r.HomeItBelongsToId == homeId)))
            .Returns(newHomeWithRoom);

        var homeWithRoom = _homeService.AddRoomToHome(homeId, roomName);

        homeWithRoom.Id.Should().Be(homeId);
        homeWithRoom.Rooms.Should().NotBeEmpty();
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void AddRoomToHome_WithNullOrEmptyName_ShouldThrowArgumentNullException(string roomName)
    {
        _homeRepository
            .Setup(act => act.HomeExists(It.IsAny<Guid>()))
            .Returns(true);

        var act = () => _homeService.AddRoomToHome(It.IsAny<Guid>(), roomName);

        act.Should().Throw<ArgumentNullException>("Room name cannot be null or empty");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddRoomToHome_WithNonExistentHome_ShouldThrowKeyNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var roomName = "any room";

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(false);

        var act = () => _homeService.AddRoomToHome(homeId, roomName);

        act.Should().Throw<KeyNotFoundException>("Home does not exist");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddRoomToHome_WithAlreadyExistentRoomName_ShouldThrowInvalidOperationException()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var roomName = "new room";
        var repeatedRoomName = "new room";

        var newRoom = new Room
        {
            Id = roomId,
            Name = roomName
        };

        var newHomeWithRoom = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = [newRoom]
        };

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(act => act.GetHomeById(homeId))
            .Returns(newHomeWithRoom);

        var act = () => _homeService.AddRoomToHome(homeId, repeatedRoomName);

        act.Should().Throw<InvalidOperationException>("Room with that name already exists in the home");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithCorrectData_ShouldContainHomeDevice()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var roomName = "new room";

        var newRoom = new Room
        {
            Id = roomId,
            HomeItBelongsToId = homeId,
            Name = roomName
        };

        var newHomeWithRoom = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = [newRoom]
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = homeId,
            HardwareId = hardwareId
        };

        _homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(newHomeDevice.HardwareId))
            .Returns(newHomeDevice);

        _homeRepository
            .Setup(act => act.GetRoomById(roomId))
            .Returns(newRoom);

        _homeRepository
            .Setup(act => act.AddDeviceToRoom(newHomeDevice, newRoom))
            .Returns(It.Is<Room>(r => r.HomeDevices.Contains(newHomeDevice)));

        var roomWithDevice = _homeService.AddDeviceToRoomOfHome(roomId, hardwareId.ToString());

        roomWithDevice.HomeDevices.Should().Contain(newHomeDevice);
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithNonExistingRoomId_ShouldThrowKeyNotFoundException()
    {
        var roomId = Guid.NewGuid();

        Room? nonExistentRoom = null;

        _homeRepository
            .Setup(act => act.GetRoomById(roomId))
            .Returns(nonExistentRoom);

        var act = () => _homeService.AddDeviceToRoomOfHome(roomId, roomId.ToString());

        act.Should().Throw<KeyNotFoundException>("Room does not exist");
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithNonExistingHardwareId_ShouldThrowKeyNotFoundException()
    {
        var hardwareId = Guid.NewGuid();

        HomeDevice? nonExistentDevice = null;

        _homeRepository
            .Setup(act => act.GetRoomById(It.IsAny<Guid>()))
            .Returns(It.IsAny<Room>());

        _homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(hardwareId))
            .Returns(nonExistentDevice);

        var act = () => _homeService.AddDeviceToRoomOfHome(It.IsAny<Guid>(), hardwareId.ToString());

        act.Should().Throw<KeyNotFoundException>("Home device does not exist");
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithHomeDeviceNotBeingInTheSameHomeAsTheRoom_ShouldThrowInvalidOperationException()
    {
        var homeId = Guid.NewGuid();
        var anotherHomeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var roomName = "new room";

        var newRoom = new Room
        {
            Id = roomId,
            Name = roomName
        };

        var newHomeWithRoom = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = [newRoom]
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = anotherHomeId,
            HardwareId = hardwareId
        };

        _homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(newHomeDevice.HardwareId))
            .Returns(newHomeDevice);

        _homeRepository
            .Setup(act => act.GetRoomById(roomId))
            .Returns(newRoom);

        var act = () => _homeService.AddDeviceToRoomOfHome(roomId, hardwareId.ToString());

        act.Should().Throw<InvalidOperationException>("Home device does not belong to the room's home");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithInvalidHardwareId_ShouldArgumentException()
    {
        var invalidHardwareId = "invalid";

        var act = () => _homeService.AddDeviceToRoomOfHome(It.IsAny<Guid>(), invalidHardwareId);

        act.Should().Throw<ArgumentException>("Home device format is invalid");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithAlreadyAddedDevice_ShouldThrowInvalidOperationException()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();

        var roomName = "new room";

        var newRoom = new Room
        {
            Id = roomId,
            HomeItBelongsToId = homeId,
            Name = roomName
        };

        var newHomeWithRoom = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Rooms = [newRoom]
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = homeId,
            HardwareId = hardwareId
        };

        _homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(newHomeDevice.HardwareId))
            .Returns(newHomeDevice);

        _homeRepository
            .Setup(act => act.GetRoomById(roomId))
            .Returns(newRoom);

        _homeRepository
            .Setup(act => act.AddDeviceToRoom(newHomeDevice, newRoom))
            .Returns(It.Is<Room>(r => r.HomeDevices.Contains(newHomeDevice)));

        _homeService.AddDeviceToRoomOfHome(roomId, hardwareId.ToString());

        _homeDeviceRepository
            .Setup(act => act.GetHomeDeviceByHardwareId(newHomeDevice.HardwareId))
            .Returns(newHomeDevice);

        _homeRepository
            .Setup(act => act.GetRoomById(roomId))
            .Returns(newRoom);

        var act = () => _homeService.AddDeviceToRoomOfHome(roomId, hardwareId.ToString());

        act.Should().Throw<InvalidOperationException>("Home device already is in room");
    }

    [TestMethod]
    public void AddAliasToHome_WithCorrectData_ShouldAddCorrectlyToHome()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var alias = "My Home Alias";

        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "email@email.com",
            Address = new Address("Main St", 123),
            Location = new GeographicLocation("50.0435436", "41.5437895"),
            MaxAmountOfMembers = 5,
            Alias = "oldAlias"
        };

        _homeRepository
            .Setup(repo => repo.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(repo => repo.GetHomeById(homeId))
            .Returns(home);

        _homeRepository
            .Setup(repo => repo.UpdateHomeAlias(It.IsAny<Home>()))
            .Callback<Home>(h => home.Alias = alias)
            .Returns(home);

        // Act
        var updatedHome = _homeService.AddAliasToHome(homeId, alias);

        // Assert
        updatedHome.Alias.Should().Be(alias);
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddAliasToHome_WithEmptyAlias_ShouldThrowArgumentNullException()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var alias = string.Empty;

        // Act
        var act = () => _homeService.AddAliasToHome(homeId, alias);

        // Assert
        act.Should().Throw<ArgumentNullException>("Alias cannot be empty");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddAliasToHome_WithNonExistentHome_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var alias = "My Home Alias";

        _homeRepository
            .Setup(repo => repo.HomeExists(homeId))
            .Returns(false);

        // Act
        var act = () => _homeService.AddAliasToHome(homeId, alias);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home does not exist");
        _homeRepository.Verify(repo => repo.HomeExists(homeId), Times.Once);
    }

    [TestMethod]
    public void CreateHomeWithAlias_WithCorrectData_ShouldCreateCorrectly()
    {
        var homeId = Guid.NewGuid();

        // Arrange
        var newHome = new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            "50.0435436",
            "41.5437895",
            5,
            "My Home Alias");

        var homeOwner = new HomeOwner { Email = "Email" };

        _homeRepository
            .Setup(repo => repo.GetHomeOwnerByEmail("Email"))
            .Returns(homeOwner);

        _homeRepository
        .Setup(act => act.Add(It.Is<Home>(h =>
            h.HomeOwner.Email == newHome.OwnerEmail &&
            h.Address.MainStreet == newHome.MainStreet &&
            h.Address.DoorNumber == newHome.DoorNumber &&
            h.Location.Latitude == newHome.Latitude &&
            h.Location.Longitude == newHome.Longitude &&
            h.MaxAmountOfMembers == newHome.MaxAmountOfMembers &&
            h.Alias == newHome.Alias)))
        .Returns(new Home { Id = homeId, HomeOwner = homeOwner });

        _homeRepository
            .Setup(act => act.GetAddDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetListDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetChangeAliasOfDevicesOfHomePermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.GetReceiveNotificationsPermission())
            .Returns(It.IsAny<Permission>());
        _homeRepository
            .Setup(act => act.AddMemberToHome(It.IsAny<Guid>(), It.IsAny<Member>()))
            .Returns(It.IsAny<Member>());

        // Act
        var result = _homeService.Create(newHome);

        // Assert
        result.Id.Should().NotBeEmpty();
        Guid.TryParse(result.Id.ToString(), out var _).Should().BeTrue();

        result.HomeOwner.Email.Should().Be(newHome.OwnerEmail);
        result.MaxAmountOfMembers.Should().Be(newHome.MaxAmountOfMembers);
        result.Alias.Should().Be(newHome.Alias);
    }

    [TestMethod]
    public void CreateHomeWithAlias_WithEmptyAlias_ShouldThrowArgumentNullException()
    {
        var act = () => new CreateHomeArgs(
            "Email",
            "MainSt",
            1111,
            "50.0435436",
            "41.5437895",
            5,
            string.Empty);

        act.Should().Throw<ArgumentNullException>("Alias cannot be empty");
    }

    [TestMethod]
    public void GetHomeByHomeOwnerId_WithCorrectData_ShouldReturnHomeId()
    {
        // Arrange
        var homeOwnerId = Guid.NewGuid();
        var home = new Home { Id = Guid.NewGuid() };

        _homeRepository
            .Setup(repo => repo.GetHomeByHomeOwnerId(homeOwnerId))
            .Returns(home);

        // Act

        var result = _homeService.GetHomeByHomeOwnerId(homeOwnerId);

        // Assert

        result.Should().Be(home);
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHomeByHomeOwnerId_WithNonExistentHome_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var homeOwnerId = Guid.NewGuid();

        _homeRepository
            .Setup(repo => repo.GetHomeByHomeOwnerId(homeOwnerId))
            .Returns((Home?)null);

        // Act
        var act = () => _homeService.GetHomeByHomeOwnerId(homeOwnerId);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home not found");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHomesThatLoggedInUserBelongsTo_WithCorrectData_ShouldReturnOneHome()
    {
        var sessionToken = "token";
        var userId = Guid.NewGuid();
        var homeId = Guid.NewGuid();

        var loggedUser = new HomeOwner
        {
            Id = userId
        };

        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "email",
            Alias = "Alias",
            Members = [new Member { AssociatedHomeOwnerId = userId, Permissions = [new Permission { Name = "list-devices-of-specific-home" }] }]
        };

        var homeDto = new GetHomesThatUserBelongsInArgs(
            home.Id.ToString(), home.Alias, home.Members.First(m => m.AssociatedHomeOwnerId == loggedUser.Id).Permissions.ConvertAll(p => p.Name), false);

        _sessionService
            .Setup(act => act.GetUserByToken(sessionToken))
            .Returns(loggedUser);

        _homeRepository
            .Setup(act => act.GetHomesThatUserIsInById(userId))
            .Returns([home]);

        var result = _homeService.GetHomesThatLoggedInUserBelongsTo(sessionToken);

        result.Should().BeEquivalentTo([homeDto]);
    }

    [TestMethod]
    public void GetHomesThatLoggedInUserIsOwnerOf_WithCorrectData_ShouldReturnOneHome()
    {
        var sessionToken = "token";
        var userId = Guid.NewGuid();
        var homeId = Guid.NewGuid();

        var loggedUser = new HomeOwner
        {
            Id = userId,
            Email = "user@email.com"
        };

        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "user@email.com",
            Alias = "Alias",
            Members =
            [
                new Member
                {
                    AssociatedHomeOwnerId = userId,
                    Permissions =
                    [
                        new Permission { Name = "list-devices-of-specific-home" },
                        new Permission { Name = "add-device-to-specific-home" },
                        new Permission { Name = "receive-notifications" },
                        new Permission { Name = "change-alias-of-specific-device" }
                        ]
                }

            ]
        };

        var homeDto = new GetHomesThatUserBelongsInArgs(
            home.Id.ToString(), home.Alias, home.Members.First(m => m.AssociatedHomeOwnerId == loggedUser.Id).Permissions.ConvertAll(p => p.Name), true);

        _sessionService
            .Setup(act => act.GetUserByToken(sessionToken))
            .Returns(loggedUser);

        _homeRepository
            .Setup(act => act.GetHomesThatUserIsInById(userId))
            .Returns([home]);

        var result = _homeService.GetHomesThatLoggedInUserBelongsTo(sessionToken);

        result.Should().BeEquivalentTo([homeDto]);
    }

    [TestMethod]
    public void GetAllRoomsOfHome_WithCorrectData_ShouldReturnAllRooms()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var roomName = "Room";

        var room = new Room
        {
            Id = roomId,
            Name = roomName
        };

        var home = new Home
        {
            Id = homeId,
            Rooms = [room]
        };

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(true);

        _homeRepository
            .Setup(act => act.GetAllRoomsOfAHome(homeId))
            .Returns([room]);

        var result = _homeService.GetAllRoomsOfHome(homeId);

        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result[0].RoomId.Should().Be(room.Id.ToString());
    }

    [TestMethod]
    public void GetAllRoomsOfHome_WithNonExistentHome_ShouldThrowKeyNotFoundException()
    {
        var homeId = Guid.NewGuid();

        _homeRepository
            .Setup(act => act.HomeExists(homeId))
            .Returns(false);

        var act = () => _homeService.GetAllRoomsOfHome(homeId);

        act.Should().Throw<KeyNotFoundException>("Home does not exist");
    }
}
