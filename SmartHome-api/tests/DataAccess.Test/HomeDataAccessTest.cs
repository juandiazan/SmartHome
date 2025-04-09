using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test;

[TestClass]
public class HomeDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly HomeRepository _repository;

    public HomeDataAccessTest()
    {
        _repository = new HomeRepository(_dbContext);
    }

    [TestInitialize]
    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithCorrectData_ShouldBeInDatabase()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        _repository.Add(newHome);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var homes = otherDbContext.Homes.ToList();

        homes.Count.Should().Be(1);
        homes[0].OwnerEmail.Should().Be("Email@gmail.com");
    }

    [TestMethod]
    public void AssociateDevice_WithCorrectHardwareId_ShouldAssociateCorrectly()
    {
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newDevice = new Device
        {
            DeviceName = "Test",
            DeviceModel = "Test123",
            Description = "Test",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        _repository.Add(newHome);
        _dbContext.Add(newCompany);
        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();

        var newHomeDevice = new HomeDevice
        {
            HomeId = newHome.Id,
            DeviceId = newDevice.Id,
            Alias = "alias",
            ConnectionState = false
        };

        _dbContext.Add(newHomeDevice);

        _dbContext.SaveChanges();

        _repository.AssociateDevice(newHome.Id, newHomeDevice.HardwareId);

        _repository.IsAssociatedToAnyHome(newHomeDevice.HardwareId).Should().BeTrue();
        newHomeDevice.HomeId.Should().Be(newHome.Id);
    }

    [TestMethod]
    public void GetHomeIdByHardwareId_WithCorrectData_ShouldReturnHomeId()
    {
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var newDevice = new Device
        {
            DeviceName = "Test",
            DeviceModel = "Test123",
            Description = "Test",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        _dbContext.Add(newCompany);
        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();

        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        _repository.Add(newHome);

        var newHomeDevice = new HomeDevice
        {
            HomeId = newHome.Id,
            DeviceId = newDevice.Id,
            Alias = "alias",
            ConnectionState = false
        };
        _dbContext.Add(newHomeDevice);

        _dbContext.SaveChanges();

        _repository.AssociateDevice(newHome.Id, newHomeDevice.HardwareId);

        var homeId = _repository.GetHomeIdByHardwareId(newHomeDevice.HardwareId);

        homeId.Should().Be(newHome.Id);
    }

    [TestMethod]
    public void GetHomeById_WithCorrectData_ShouldReturnHome()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        _repository.Add(newHome);

        var home = _repository.GetHomeById(newHome.Id);

        home.Should().BeEquivalentTo(newHome);
    }

    [TestMethod]
    public void AddMemberToHome_WithCorrectData_ShouldAddMemberToHome()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            Members = [],
            HomeOwner = homeOwner
        };

        _dbContext.Add(newHome);
        _dbContext.SaveChanges();

        var trackedHome = _dbContext.Homes.First(h => h.Id == newHome.Id);

        var newMember = new Member
        {
            Id = Guid.NewGuid(),
            Permissions = [new Permission { Name = "add-device-to-specific-home" }],
            AssociatedHomeOwner = homeOwner
        };

        _repository.AddMemberToHome(trackedHome.Id, newMember);
        trackedHome.Members.Should().Contain(newMember);
    }

    [TestMethod]
    public void ListMembers_WithCorrectData_ShouldReturnMember()
    {
        var newHomeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        _dbContext.HomeOwners.Add(newHomeOwner);

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            Members =
            [
                new Member
                {
                    Id = newHomeOwner.Id,
                    Permissions =
                    [
                        new Permission { Name = "recieve-notifications" }
                    ],
                    AssociatedHomeOwner = newHomeOwner
                }

            ],
            HomeOwner = newHomeOwner
        };

        _repository.Add(newHome);

        var members = _repository.ListMembersOfHome(newHome.Id);

        members.Should().NotBeNull();
        members.Should().HaveCount(1);
    }

    [TestMethod]
    public void UpdateMemberNotifications_EnableNotifications_ShouldAddPermission()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var homeId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "owner@example.com",
            Address = new Address("Main Street", 123),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner,
        };
        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        var member = new Member
        {
            Id = memberId,
            Permissions = [],
            AssociatedHomeOwner = homeOwner
        };

        _dbContext.Members.Add(member);
        _dbContext.SaveChanges();

        home.Members.Add(member);
        _dbContext.Update(home);
        _dbContext.SaveChanges();

        member.Permissions.Add(_dbContext.Permissions.FirstOrDefault(p => p.Id == SmartHomeDBContext.ReceiveNotificationsPermissionId)!);

        _repository.UpdateMemberNotifications(member);

        var updatedMember = _dbContext.Members
            .Include(m => m.Permissions)
            .FirstOrDefault(x => x.Id == memberId);

        updatedMember!.Permissions.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetHomeMembers_WithCorrectData_ShouldReturnMembers()
    {
        // Arrange

        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };
        var homeId = Guid.NewGuid();
        var member = new Member { Id = Guid.NewGuid(), AssociatedHomeOwner = homeOwner };
        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "owner@example.com", // Ensure OwnerEmail is set
            Address = new Address("Main Street", 123),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            Members = [member],
            HomeOwner = homeOwner
        };
        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        // Act
        var result = _repository.GetHomeMembers(homeId);

        // Assert
        result.Should().ContainSingle().Which.Should().BeEquivalentTo(member);
    }

    [TestMethod]
    public void ListHomeDevices_WithCorrectData_ShouldReturnHomeDevices()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "owner@owner.com",
            Address = new Address("Main Street", 123),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        var company = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = Guid.NewGuid().ToString()
        };

        var device = new Device
        {
            Id = Guid.NewGuid(),
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = Guid.NewGuid(),
            CompanyItIsAssociatedTo = company
        };

        var homeDevice = new HomeDevice { HomeId = homeId, DeviceId = device.Id, ConnectionState = false, Alias = "alias" };

        _dbContext.Homes.Add(home);
        _dbContext.Companies.Add(company);
        _dbContext.Devices.Add(device);

        _dbContext.SaveChanges();

        _dbContext.HomeDevices.Add(homeDevice);

        _dbContext.SaveChanges();

        var result = _repository.ListHomeDevices(homeId, null);

        result.Should().ContainSingle().Which.Should().BeEquivalentTo(homeDevice);
    }

    [TestMethod]
    public void ListHomeDevices_WithFilters_ShouldReturnOneDevice()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            OwnerEmail = "owner@owner.com",
            Address = new Address("Main Street", 123),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };
        var room = new Room
        {
            HomeItBelongsTo = home,
            Name = "Name"
        };
        var anotherRoom = new Room
        {
            HomeItBelongsTo = home,
            Name = "Another"
        };
        home.AddRoom(room);
        home.AddRoom(anotherRoom);
        var company = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = Guid.NewGuid().ToString()
        };

        var device = new Device
        {
            Id = Guid.NewGuid(),
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = Guid.NewGuid(),
            CompanyItIsAssociatedTo = company
        };

        var homeDevice = new HomeDevice { HomeId = homeId, DeviceId = device.Id, ConnectionState = false, Alias = "alias", RoomItIsIn = room };
        var anotherHomeDevice = new HomeDevice { HomeId = homeId, DeviceId = device.Id, ConnectionState = true, Alias = "alias", RoomItIsIn = anotherRoom };

        _dbContext.Homes.Add(home);
        _dbContext.Companies.Add(company);
        _dbContext.Devices.Add(device);

        _dbContext.SaveChanges();

        _dbContext.HomeDevices.Add(homeDevice);
        _dbContext.HomeDevices.Add(anotherHomeDevice);

        _dbContext.SaveChanges();

        var result = _repository.ListHomeDevices(homeId, "Name");

        result.Should().ContainSingle().Which.Should().BeEquivalentTo(homeDevice);
    }

    [TestMethod]
    public void AddRoomToHome_WithCorrectData_ShouldAddSuccessfully()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var newHome = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            Members = [],
            HomeOwner = homeOwner
        };

        _dbContext.Add(newHome);
        _dbContext.SaveChanges();

        var newRoom = new Room
        {
            Name = "new room",
            HomeItBelongsToId = newHome.Id
        };

        newHome.AddRoom(newRoom);

        _repository.UpdateHome(newHome, newRoom);

        var home = _repository.GetHomeById(newHome.Id);

        home.Rooms.Should().Contain(newRoom);
    }

    [TestMethod]
    public void AddHomeDeviceToRoom_WithCorrectData_ShouldContainHomeDevice()
    {
        var homeId = Guid.NewGuid();

        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        _dbContext.Add(homeOwner);
        _dbContext.SaveChanges();

        var newRoom = new Room
        {
            Name = "name",
            HomeItBelongsToId = homeId
        };

        var newHome = new Home
        {
            Id = homeId,
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            Members = [],
            Rooms = [newRoom],
            HomeOwner = homeOwner
        };

        _dbContext.Add(newHome);
        _dbContext.SaveChanges();

        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        _dbContext.Add(newCompany);
        _dbContext.SaveChanges();

        var newDevice = new Device
        {
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();

        var newHomeDevice = new HomeDevice
        {
            DeviceId = newDevice.Id,
            HomeId = newHome.Id,
            Alias = "alias"
        };

        _dbContext.Add(newHomeDevice);
        _dbContext.SaveChanges();

        newRoom.AddHomeDevice(newHomeDevice);
        newHomeDevice.RoomItIsInId = newRoom.Id;

        _repository.AddDeviceToRoom(newHomeDevice, newRoom);

        _repository.GetRoomById(newRoom.Id)!.HomeDevices.Should().Contain(newHomeDevice);
    }

    [TestMethod]
    public void UpdateHomeAlias_WithCorrectData_ShouldUpdateAlias()
    {
        // Arrange
        var newAlias = "New Home Alias";
        var home = new Home
        {
            Id = Guid.NewGuid(),
            OwnerEmail = "owner@example.com",
            Address = new Address("Main Street", 123),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = new HomeOwner
            {
                Name = "Name",
                Surname = "Surname",
                Email = "Email@gmail.com",
                Password = "Passw1rd",
                ProfilePicture = "ProfilePicture",
                RoleId = SmartHomeDBContext.HomeOwnerRoleId,
                Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
            }
        };

        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        // Act
        home.Alias = newAlias;
        _repository.UpdateHomeAlias(home);

        // Assert
        var updatedHome = _dbContext.Homes.First(h => h.Id == home.Id);
        updatedHome.Alias.Should().Be(newAlias);
    }

    [TestMethod]
    public void GetHomeByHomeOwnerId_WithCorrectData_ShouldReturnHome()
    {
        // Arrange
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var home = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        // Act
        var result = _repository.GetHomeByHomeOwnerId(homeOwner.Id);

        // Assert
        result.Should().BeEquivalentTo(home);
    }

    [TestMethod]
    public void GetHomesThatUserIsInById_WithCorrectUser_ShouldReturnTwoHomes()
    {
        var homeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var anotherHomeOwner = new HomeOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "anotheremail@gmail.com",
            Password = "Passw1rd",
            ProfilePicture = "ProfilePicture",
            RoleId = SmartHomeDBContext.HomeOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.HomeOwnerRoleId)
        };

        var home = new Home
        {
            OwnerEmail = "Email@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = homeOwner
        };

        var anotherHome = new Home
        {
            OwnerEmail = "anotheremail@gmail.com",
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            HomeOwner = anotherHomeOwner,
            Members = [new Member { Id = Guid.NewGuid(), AssociatedHomeOwner = homeOwner }]
        };

        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        _dbContext.Homes.Add(anotherHome);
        _dbContext.SaveChanges();

        var homes = _repository.GetHomesThatUserIsInById(homeOwner.Id);

        homes.Should().Contain(home);
        homes.Should().Contain(anotherHome);
    }

    [TestMethod]
    public void GetRoomsOfHome_ShouldReturnRooms()
    {
        var home = new Home
        {
            HomeOwner = new HomeOwner
            {
                Name = "Name",
                Surname = "Surname",
                Email = "email",
                Password = "password",
                RoleId = SmartHomeDBContext.HomeOwnerRoleId
            },
            Address = new Address("Street", 1111),
            Location = new GeographicLocation("123", "456"),
            MaxAmountOfMembers = 5,
            OwnerEmail = "email"
        };

        var aRoom = new Room
        {
            HomeItBelongsToId = home.Id,
            Name = "Room"
        };

        var anotherRoom = new Room
        {
            HomeItBelongsToId = home.Id,
            Name = "Another Room"
        };

        home.Rooms.Add(aRoom);
        home.Rooms.Add(anotherRoom);

        _dbContext.Homes.Add(home);
        _dbContext.SaveChanges();

        var result = _repository.GetAllRoomsOfAHome(home.Id);

        result.Should().Contain(aRoom);
        result.Should().Contain(anotherRoom);
    }

    [TestMethod]
    public void GetChangeAliasPermission_ShouldReturnCorrectPermission()
    {
        var permission = _repository.GetChangeAliasOfDevicesOfHomePermission();

        permission.Id.Should().Be(SmartHomeDBContext.ChangeHomeDeviceAliasPermissionId);
        permission.Name.Should().Be("change-alias-of-specific-device");
    }
}
