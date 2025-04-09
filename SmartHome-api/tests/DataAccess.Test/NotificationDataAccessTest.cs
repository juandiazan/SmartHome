using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters.Models;

namespace DataAccess.Test;

[TestClass]
public class NotificationDataAccessTest
{
    private readonly SmartHomeDBContext _context = DbContextBuilder.BuildTestDbContext();
    private readonly NotificationRepository _repository = null!;

    public NotificationDataAccessTest()
    {
        _repository = new NotificationRepository(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithCorrectData_ShouldBeInDatabase()
    {
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        var device = new Device
        {
            DeviceName = "name",
            DeviceModel = "model",
            Description = "description",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var homeOwner = new HomeOwner
        {
            Surname = "Surname",
            Password = "Passw1rd",
            Id = Guid.NewGuid(),
            Email = "Email@email.com",
            Name = "Name",
            ProfilePicture = "ProfilePicture",
            Role = new Role { RoleName = "home-owner" }
        };

        _context.Companies.Add(newCompany);
        _context.Devices.Add(device);

        var homeDevice = new HomeDevice
        {
            DeviceId = device.Id,
            Alias = "alias"
        };
        _context.HomeDevices.Add(homeDevice);

        var home = new Home
        {
            OwnerEmail = "Email",
            Address = new Address("MainSt", 1111),
            Location = new GeographicLocation("55", "55"),
            HomeOwner = homeOwner
        };

        home.AssociateDevice(homeDevice);

        _context.Homes.Add(home);

        _context.SaveChanges();

        var user = Guid.NewGuid();

        var newNotif = new Notification
        {
            HomeId = home.Id,
            TriggeringDeviceId = homeDevice.HardwareId,
            TriggeringEvent = "Person detected",
            UserItIsAddressedToId = user
        };

        _repository.Add(newNotif);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var notifications = otherDbContext.Notifications
            .Include(n => n.Home)
            .Include(n => n.TriggeringDevice)
            .ToList();

        notifications.Count.Should().Be(1);
        notifications[0].Id.Should().Be(newNotif.Id);
        notifications[0].Home.Id.Should().Be(home.Id);
        notifications[0].TriggeringDevice.HardwareId.Should().Be(homeDevice.HardwareId);
    }

    [TestMethod]
    public void GetUserNotifications_WithValidParameters_ShouldReturnCorrectNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        var homeOwner = new HomeOwner
        {
            Surname = "Surname",
            Password = "Passw1rd",
            Id = Guid.NewGuid(),
            Email = "Email@email.com",
            Name = "Name",
            ProfilePicture = "ProfilePicture",
            Role = new Role { RoleName = "home-owner" },
        };

        var device = new Device
        {
            DeviceName = "name",
            DeviceModel = "model",
            Description = "description",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        _context.Companies.Add(newCompany);
        _context.Devices.Add(device);

        var homeDevice = new HomeDevice
        {
            DeviceId = device.Id,
            Alias = "alias"
        };
        _context.HomeDevices.Add(homeDevice);

        var home = new Home
        {
            OwnerEmail = "Email",
            Address = new Address("MainSt", 1111),
            Location = new GeographicLocation("55", "55"),
            HomeOwner = homeOwner
        };

        home.AssociateDevice(homeDevice);
        home.Members.Add(new Member { Id = userId, AssociatedHomeOwner = homeOwner });

        _context.Homes.Add(home);

        var newNotif = new Notification
        {
            HomeId = home.Id,
            Home = home,
            TriggeringDeviceId = homeDevice.HardwareId,
            TriggeringEvent = "Person detected",
            WasRead = false,
            UserItIsAddressedToId = userId,
        };

        _context.Notifications.Add(newNotif);
        _context.SaveChanges();

        // Act
        var notifications = _repository.GetUserNotifications(homeOwner.Id, new NotificationFilterArgs()).ToList();

        // Assert
        notifications.Should().HaveCount(1);
        notifications[0].DateTimeOfEvent.Date.Should().Be(newNotif.DateTimeOfEvent.Date);
        notifications[0].TriggeringDevice.Device!.DeviceModel.Should().Be(newNotif.TriggeringDevice.Device!.DeviceModel);
    }

    [TestMethod]
    public void GetUserNotifications_WithValidFilters_ShouldReturnOneNotification()
    {
        var userId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        var homeOwner = new HomeOwner
        {
            Surname = "Surname",
            Password = "Passw1rd",
            Id = Guid.NewGuid(),
            Email = "Email@email.com",
            Name = "Name",
            ProfilePicture = "ProfilePicture",
            Role = new Role { RoleName = "home-owner" },
        };
        var device = new Device
        {
            DeviceName = "name",
            DeviceModel = "model",
            Description = "description",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };
        _context.Companies.Add(newCompany);
        _context.Devices.Add(device);
        var homeDevice = new HomeDevice
        {
            DeviceId = device.Id,
            Alias = "alias"
        };
        _context.HomeDevices.Add(homeDevice);
        var home = new Home
        {
            OwnerEmail = "Email",
            Address = new Address("MainSt", 1111),
            Location = new GeographicLocation("55", "55"),
            HomeOwner = homeOwner
        };
        home.AssociateDevice(homeDevice);
        home.Members.Add(new Member { Id = memberId, AssociatedHomeOwnerId = homeOwner.Id, AssociatedHomeOwner = homeOwner });
        _context.Homes.Add(home);

        var newNotif = new Notification
        {
            HomeId = home.Id,
            Home = home,
            TriggeringDeviceId = homeDevice.HardwareId,
            TriggeringEvent = "First Notif",
            WasRead = false,
            UserItIsAddressedToId = memberId,
            DateTimeOfEvent = DateTime.Today.AddDays(-1)
        };

        _context.Notifications.Add(newNotif);
        var anotherHomeOwner = new HomeOwner
        {
            Surname = "Surname",
            Password = "Passw1rd",
            Id = Guid.NewGuid(),
            Email = "anotherEmail@email.com",
            Name = "Name",
            ProfilePicture = "ProfilePicture",
            Role = new Role { RoleName = "home-owner" },
        };
        var anotherDevice = new Device
        {
            DeviceName = "name",
            DeviceModel = "model",
            Description = "description",
            Photos = [],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };
        _context.Devices.Add(anotherDevice);
        var anotherHomeDevice = new HomeDevice
        {
            DeviceId = device.Id,
            Alias = "alias2"
        };
        _context.HomeDevices.Add(anotherHomeDevice);
        var anotherHome = new Home
        {
            OwnerEmail = "SecondEmail",
            Address = new Address("MainSt2", 2222),
            Location = new GeographicLocation("66", "66"),
            HomeOwner = homeOwner
        };
        home.AssociateDevice(anotherHomeDevice);
        home.Members.Add(new Member { Id = Guid.NewGuid(), AssociatedHomeOwnerId = anotherHomeOwner.Id, AssociatedHomeOwner = anotherHomeOwner });
        _context.Homes.Add(anotherHome);

        var anotherNotif = new Notification
        {
            HomeId = anotherHome.Id,
            Home = anotherHome,
            TriggeringDeviceId = anotherHomeDevice.HardwareId,
            TriggeringEvent = "Second Notif",
            WasRead = true,
            UserItIsAddressedToId = userId,
            DateTimeOfEvent = DateTime.Today,
        };

        _context.Notifications.Add(anotherNotif);
        _context.SaveChanges();

        var notifications = _repository.GetUserNotifications(homeOwner.Id, new NotificationFilterArgs(DeviceType.Sensor, DateTime.Today.AddDays(-1), false)).ToList();

        notifications.Should().HaveCount(1);
        notifications[0].DateTimeOfEvent.Should().Be(newNotif.DateTimeOfEvent);
        notifications[0].TriggeringDevice.Device!.DeviceModel.Should().Be(newNotif.TriggeringDevice.Device!.DeviceModel);
    }
}
