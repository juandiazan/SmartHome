using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using PaginationAndFilters.Models;

namespace DataAccess.Test;

[TestClass]
public class DeviceDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly DeviceRepository _repository;

    public DeviceDataAccessTest()
    {
        _repository = new DeviceRepository(_dbContext);
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
    public void Add_WithCorrectData_ShouldAddCorrectly()
    {
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

        var anotherDevice = new Camera
        {
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        _repository.Add(newDevice);
        _repository.Add(anotherDevice);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var devices = otherDbContext.Devices.ToList();

        devices.Count.Should().Be(2);
        devices[0].DeviceModel.Should().Be("Model123");
        devices[1].DeviceType.Should().Be(DeviceType.Camera);
    }

    [TestMethod]
    public void GetAll_WithTwoElements_ShouldListTwo()
    {
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

        var anotherDevice = new Camera
        {
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs());

        entitiesSaved.Count.Should().Be(2);
        entitiesSaved[0].DeviceName.Should().Be(newDevice.DeviceName);
        entitiesSaved[1].DeviceName.Should().Be(anotherDevice.DeviceName);
    }

    [TestMethod]
    public void GetAll_WithPagination_ShouldReturnCorrectElement()
    {
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

        var anotherDevice = new Camera
        {
            DeviceName = "DeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs(1, 1));

        entitiesSaved.Count.Should().Be(1);
        entitiesSaved[0].DeviceName.Should().Be(newDevice.DeviceName);
    }

    [TestMethod]
    public void GetAll_FilterByDeviceName_ShouldReturnCorrectElement()
    {
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
            DeviceName = "ExpectedDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var anotherDevice = new Camera
        {
            DeviceName = "AnotherDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs(deviceName: "ExpectedDeviceName"));

        entitiesSaved.Count.Should().Be(1);
        entitiesSaved[0].DeviceName.Should().Be(newDevice.DeviceName);
    }

    [TestMethod]
    public void GetAll_FilterByDeviceModel_ShouldReturnCorrectElement()
    {
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
            DeviceName = "ExpectedDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var anotherDevice = new Camera
        {
            DeviceName = "AnotherDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs(model: "Model123"));

        entitiesSaved.Count.Should().Be(2);
    }

    [TestMethod]
    public void GetAll_FilterByOwnerCompanyName_ShouldReturnCorrectElement()
    {
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
            DeviceName = "ExpectedDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var anotherDevice = new Camera
        {
            DeviceName = "AnotherDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs(companyName: "ExpectedCompanyName"));

        entitiesSaved.Count.Should().Be(2);
    }

    [TestMethod]
    public void GetAll_FilterByDeviceType_ShouldReturnCorrectElement()
    {
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
            DeviceName = "ExpectedDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Sensor,
            CompanyId = newCompany.Id
        };

        var anotherDevice = new Camera
        {
            DeviceName = "AnotherDeviceName",
            DeviceModel = "Model123",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = newCompany.Id
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(newDevice);
        context.Add(anotherDevice);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new DeviceFilterArgs(deviceType: DeviceType.Camera));

        entitiesSaved.Count.Should().Be(1);
    }
}
