using System.Net;
using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class HomeControllerTest
{
    private Mock<IHomeService> _homeService = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<IHomeDeviceService> _homeDeviceService = null!;
    private HomeController _homeController = null!;

    private string token = null!;
    private HomeOwner userInSession = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeService = new Mock<IHomeService>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _homeDeviceService = new Mock<IHomeDeviceService>(MockBehavior.Strict);
        _homeController = new HomeController(_homeService.Object, _sessionService.Object, _homeDeviceService.Object);

        token = "token";

        userInSession = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "email@email.com",
            Password = "Password123!",
            ProfilePicture = "profile.jpg",
            Role = new Role { RoleName = "home-owner" }
        };

        _sessionService
            .Setup(act => act.GetUserByToken(token))
            .Returns(userInSession);
    }

    [TestMethod]
    public void CreateHome_WithCorrectData_ShouldCreate()
    {
        var request = new CreateHomeRequest("Address", 12, "-1.100.0", "1.1000.2", 2, "alias");
        var ownerEmail = "email@email.com";

        var createHomeArgs = request.ToArgs(ownerEmail);

        _homeService
            .Setup(act => act.Create(It.Is<CreateHomeArgs>(args =>
                args.OwnerEmail == createHomeArgs.OwnerEmail &&
                args.MainStreet == createHomeArgs.MainStreet &&
                args.DoorNumber == createHomeArgs.DoorNumber &&
                args.Latitude == createHomeArgs.Latitude &&
                args.Longitude == createHomeArgs.Longitude &&
                args.MaxAmountOfMembers == createHomeArgs.MaxAmountOfMembers)))
            .Returns(new Home
            {
                OwnerEmail = ownerEmail,
                Address = new Address(request.MainStreet!, request.DoorNumber!.Value),
                Location = new GeographicLocation(request.Latitude!, request.Longitude!),
                MaxAmountOfMembers = request.MaxAmountOfMembers!.Value
            });

        var result = _homeController.CreateHome(request, token) as CreatedResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void ListHomeDevices_WithCorrectData_ShouldReturnHomeDevicesList()
    {
        var homeId = Guid.NewGuid();
        var homeDevices = new List<GetAllDevicesOfHomeArgs>
            {
                new("Model", "Name", "Photo", true, "firstAlias", Guid.NewGuid().ToString(), "None"),
                new("AnotherModel", "AnotherName", "AnotherPhoto", true, "secondAlias", Guid.NewGuid().ToString(), "None")
            };

        _homeService
            .Setup(act => act.ListHomeDevices(homeId, null))
            .Returns(homeDevices);

        var result = _homeController.ListHomeDevices(homeId, null);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void AddMember_WithAllPermissions_ShouldAddMemberToHome()
    {
        var homeToBeAddedToId = Guid.NewGuid();
        var newMemberToAddRequest = new NewMemberToAddRequest("newMember@gmail.com", true, true, true);
        var newMemberToAddArgs = newMemberToAddRequest.ToArgs();
        var homeOwner = new HomeOwner
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Surname = "Surname",
            Email = "newMember@gmail.com",
            ProfilePicture = "profile.jpg",
            Role = new Role { RoleName = "home-owner" }
        };

        var newMemberAdded = new Member
        {
            Id = Guid.NewGuid(),
            AssociatedHomeOwner = homeOwner
        };

        _homeService
            .Setup(act => act.AddMemberToHome(homeToBeAddedToId, It.Is<AddMemberToHomeArgs>(args =>
                args.EmailOfNewMember == newMemberToAddArgs.EmailOfNewMember &&
                args.CanAddDeviceToHome == newMemberToAddArgs.CanAddDeviceToHome &&
                args.CanSeeDevicesOfHome == newMemberToAddArgs.CanSeeDevicesOfHome)))
            .Returns(newMemberAdded);

        var result = _homeController.AddMemberToHome(homeToBeAddedToId, newMemberToAddRequest);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void AddMember_WithIncorrectFormatEmail_ShouldBadRequest()
    {
        var homeToBeAddedToId = Guid.NewGuid();
        var newMemberToAddRequest = new NewMemberToAddRequest("newMember-with-incorrect-format", true, true, true);
        var newMemberToAddArgs = newMemberToAddRequest.ToArgs();

        _homeService
            .Setup(act => act.AddMemberToHome(homeToBeAddedToId, It.Is<AddMemberToHomeArgs>(args =>
                args.EmailOfNewMember == newMemberToAddArgs.EmailOfNewMember &&
                args.CanAddDeviceToHome == newMemberToAddArgs.CanAddDeviceToHome &&
                args.CanSeeDevicesOfHome == newMemberToAddArgs.CanSeeDevicesOfHome)))
            .Throws(new ArgumentException("Invalid email format"));

        Action act = () => _homeController.AddMemberToHome(homeToBeAddedToId, newMemberToAddRequest);
        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format");
    }

    [TestMethod]
    public void AddMember_WithNonExistingHomeId_ShouldNotFound()
    {
        var homeToBeAddedToId = Guid.NewGuid();
        var newMemberToAddRequest = new NewMemberToAddRequest("newMember@gmail.com", true, true, true);
        var newMemberToAddArgs = newMemberToAddRequest.ToArgs();

        _homeService
            .Setup(act => act.AddMemberToHome(homeToBeAddedToId, It.Is<AddMemberToHomeArgs>(args =>
                args.EmailOfNewMember == newMemberToAddArgs.EmailOfNewMember &&
                args.CanAddDeviceToHome == newMemberToAddArgs.CanAddDeviceToHome &&
                args.CanSeeDevicesOfHome == newMemberToAddArgs.CanSeeDevicesOfHome)))
            .Throws(new KeyNotFoundException("Home not found"));

        Action act = () => _homeController.AddMemberToHome(homeToBeAddedToId, newMemberToAddRequest);
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home not found");
    }

    [TestMethod]
    public void AddDeviceToHome_WithCorrectData_ShouldAddDeviceToHome()
    {
        var homeId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var hdId = Guid.NewGuid();
        var alias = "alias";

        var hdArgs = new CreateHomeDeviceArgs(homeId, deviceId, alias);

        var deviceAdded = new HomeDevice
        {
            HardwareId = hdId,
            DeviceId = deviceId,
            HomeId = homeId,
            Alias = alias
        };

        _homeDeviceService
            .Setup(act => act.Create(It.Is<CreateHomeDeviceArgs>(args =>
            args.HomeId == homeId &&
            args.DeviceId == deviceId &&
            args.HomeDeviceAlias == alias)))
            .Returns(deviceAdded);

        _homeService
            .Setup(act => act.AssociateDevice(homeId, hdId))
            .Returns(It.IsAny<HomeDevice>());

        var args = new AddDeviceToHomeRequest(deviceId.ToString(), alias);
        var result = _homeController.AddDeviceToHome(homeId, args);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void AddDeviceToHome_WithEmptyHardwareId_ShouldBadRequest()
    {
        var args = new AddDeviceToHomeRequest(Guid.Empty.ToString(), "alias");
        var homeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(act => act.Create(It.IsAny<CreateHomeDeviceArgs>()))
            .Returns(new HomeDevice());

        _homeService
            .Setup(act => act.AssociateDevice(homeId, It.IsAny<Guid>()))
            .Throws(new ArgumentException("Invalid hardware ID"));

        Action act = () => _homeController.AddDeviceToHome(homeId, args);
        act.Should().Throw<ArgumentException>().WithMessage("Invalid hardware ID");
    }

    [TestMethod]
    public void AddDeviceToHome_WithNonExistingHomeId_ShouldNotFound()
    {
        var args = new AddDeviceToHomeRequest(Guid.NewGuid().ToString(), "alias");
        var homeId = Guid.NewGuid();

        _homeDeviceService
            .Setup(act => act.Create(It.IsAny<CreateHomeDeviceArgs>()))
            .Returns(new HomeDevice());

        _homeService
            .Setup(act => act.AssociateDevice(homeId, It.IsAny<Guid>()))
            .Throws(new KeyNotFoundException("Home not found"));

        Action act = () => _homeController.AddDeviceToHome(homeId, args);
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home not found");
    }

    [TestMethod]
    public void GetMembers_WithCorrectData_ShouldReturnMembersList()
    {
        var memberID = Guid.NewGuid().ToString();
        var anotherId = Guid.NewGuid().ToString();
        var homeId = Guid.NewGuid();
        var members = new List<GetAllMembersOfHomeArgs>
            {
                new(memberID, "John Doe", "john.doe@example.com", "profile1.jpg", ["Permission1"], true),
                new(anotherId, "Jane Smith", "jane.smith@example.com", "profile2.jpg", ["Permission1"], false)
            };

        _homeService
            .Setup(act => act.ListMembersOfHome(homeId))
            .Returns(members);

        var result = _homeController.ListMembersOfHome(homeId);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void ListHomeDevices_WithNonExistingHomeId_ShouldNotFound()
    {
        var homeId = Guid.NewGuid();

        _homeService
            .Setup(act => act.ListHomeDevices(homeId, null))
            .Throws(new KeyNotFoundException("Home not found"));

        Action act = () => _homeController.ListHomeDevices(homeId, null);

        act.Should().Throw<KeyNotFoundException>().WithMessage("Home not found");
    }

    [TestMethod]
    public void UpdateMemberNotifications_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var boolToSet = true;
        var request = new UpdateMemberNotificationsRequest(boolToSet, Guid.NewGuid());

        _homeService
            .Setup(service => service.UpdateMemberNotifications(homeId, It.IsAny<UpdateMemberNotificationsArgs>()))
            .Verifiable();

        // Act
        var result = _homeController.UpdateMemberNotifications(homeId, request) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result!.Value.Should().Be("Modifications saved successfully");

        _homeService.Verify(service => service.UpdateMemberNotifications(homeId, It.IsAny<UpdateMemberNotificationsArgs>()), Times.Once);
    }

    [TestMethod]
    public void AddRoomToHome_WithValidData_ShouldReturnOk()
    {
        var homeToBeAddedToId = Guid.NewGuid();
        var newRoomToAdd = new AddRoomToHomeRequest("new room name");

        var newRoomAdded = new Room
        {
            Id = Guid.NewGuid(),
            Name = newRoomToAdd.RoomName
        };

        _homeService
            .Setup(act => act.AddRoomToHome(homeToBeAddedToId, newRoomToAdd.RoomName))
            .Returns(It.Is<Home>(h => h.Rooms.Contains(newRoomAdded)));

        var result = _homeController.AddRoomToHome(homeToBeAddedToId, newRoomToAdd);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void AddRoomToHome_WithInvalidRoomName_ShouldBadRequest()
    {
        var homeToBeAddedToId = Guid.NewGuid();
        var newRoomToAdd = new AddRoomToHomeRequest(string.Empty);

        var newRoomAdded = new Room
        {
            Id = Guid.NewGuid(),
            Name = newRoomToAdd.RoomName
        };

        _homeService
            .Setup(act => act.AddRoomToHome(homeToBeAddedToId, newRoomToAdd.RoomName))
            .Throws(new ArgumentNullException());

        var act = () => _homeController.AddRoomToHome(homeToBeAddedToId, newRoomToAdd);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddAliasToHome_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var alias = "MyHomeAlias";
        var request = new AddAliasToHomeRequest(alias);

        _homeService
            .Setup(service => service.AddAliasToHome(homeId, alias))
            .Returns(new Home { Id = homeId, Alias = alias })
            .Verifiable();

        // Act
        var result = _homeController.AddAliasToHome(homeId, request) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result!.Value.Should().Be("Alias added successfully");

        _homeService.Verify(service => service.AddAliasToHome(homeId, alias), Times.Once);
    }

    [TestMethod]
    public void GetHomes_WithCorrectToken_ShouldReturnOk()
    {
        var token = "token";

        var homes = new List<GetHomesThatUserBelongsInArgs>
        {
            new(Guid.NewGuid().ToString(), "Home1", ["list-devices-of-specific-home"], true),
            new(Guid.NewGuid().ToString(), "Home2", ["add-device-to-specific-home"], true)
        };

        _homeService
            .Setup(act => act.GetHomesThatLoggedInUserBelongsTo(token))
            .Returns(homes);

        var result = _homeController.GetHomes(token);

        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void GetRoomsOfHome_WithValidHome_ShouldReturnOk()
    {
        var homeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var rooms = new List<GetAllRoomsOfHomeArgs>() { new(roomId.ToString(), "roomName") };

        _homeService
            .Setup(act => act.GetAllRoomsOfHome(homeId))
            .Returns(rooms);

        var result = _homeController.ListRoomsOfHome(homeId);

        result.Should().BeOfType<OkObjectResult>();
    }
}
