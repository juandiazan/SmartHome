using DataAccess.DBContext;
using Domain;
using FluentAssertions;

namespace DataAccess.Test;

[TestClass]
public class HomeDeviceDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly HomeDeviceRepository _repository;

    public HomeDeviceDataAccessTest()
    {
        _repository = new HomeDeviceRepository(_dbContext);
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
    public void Create_WithCorrectData_ShouldBeInDatabase()
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
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var newHome = new Home
        {
            OwnerEmail = "email@gmail.com",
            MaxAmountOfMembers = 4,
            Location = new GeographicLocation("12.34", "56.78"),
            Address = new Address("Street", 203),
            HomeOwner = homeOwner
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = newHome.Id,
            DeviceId = newDevice.Id,
            Alias = "alias",
            ConnectionState = false
        };

        _dbContext.Add(newHome);
        _dbContext.Add(newCompany);
        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();

        _repository.Add(newHomeDevice);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var homeDevices = otherDbContext.HomeDevices.ToList();

        homeDevices.Count.Should().Be(1);
        homeDevices[0].DeviceId.Should().Be(newDevice.Id);
    }

    [TestMethod]
    public void GetHomeDeviceByHardwareId_WithCorrectData_ShouldReturnHomeDevice()
    {
        var newCompany = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = Guid.NewGuid().ToString() // Ensure unique Rut
        };

        var newDevice = new Device
        {
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
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
        var newHome = new Home
        {
            OwnerEmail = "email@email.com",
            MaxAmountOfMembers = 4,
            Location = new GeographicLocation("12.34", "56.78"),
            Address = new Address("Street", 203),
            HomeOwner = homeOwner
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = newHome.Id,
            DeviceId = newDevice.Id,
            Alias = "alias",
            ConnectionState = false
        };

        _dbContext.Add(newHome);
        _dbContext.Add(newCompany);
        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();

        _dbContext.Add(newHomeDevice);
        _dbContext.SaveChanges();

        var homeDevice = _repository.GetHomeDeviceByHardwareId(newHomeDevice.HardwareId);

        homeDevice.Should().NotBeNull();
        homeDevice.DeviceId.Should().Be(newDevice.Id);
        homeDevice.HomeId.Should().Be(newHome.Id);
        homeDevice.ConnectionState.Should().BeFalse();
    }

    [TestMethod]
    public void Update_WithCorrectData_ShouldUpdateCorrectly()
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
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var newHome = new Home
        {
            OwnerEmail = "email@gmail.com",
            MaxAmountOfMembers = 4,
            Location = new GeographicLocation("12.34", "56.78"),
            Address = new Address("Street", 203),
            HomeOwner = homeOwner
        };

        var newHomeDevice = new HomeDevice
        {
            HomeId = newHome.Id,
            DeviceId = newDevice.Id,
            Alias = "alias",
            ConnectionState = false
        };

        _dbContext.Add(newHome);
        _dbContext.Add(newCompany);
        _dbContext.Add(newDevice);
        _dbContext.SaveChanges();
        _repository.Add(newHomeDevice);

        newHomeDevice.Alias = "newAlias";
        _repository.UpdateHomeDevice(newHomeDevice);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var homeDevices = otherDbContext.HomeDevices.ToList();

        homeDevices.Count.Should().Be(1);
        homeDevices[0].DeviceId.Should().Be(newDevice.Id);
        homeDevices[0].Alias.Should().Be("newAlias");
    }
}
