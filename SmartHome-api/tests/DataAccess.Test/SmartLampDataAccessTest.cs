using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using IDataAccess;

namespace DataAccess.Test;

[TestClass]
public class SmartLampDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly ISmartLampRepository _repository;

    public SmartLampDataAccessTest()
    {
        _repository = new SmartLampRepository(_dbContext);
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
    public void Add_WithCorrectSmartLamp_ShouldAddCorrectly()
    {
        var companyOfDevice = new Company
        {
            CompanyName = "Name",
            Logotype = "Logotype",
            Rut = "Rut",
        };

        var newSmartLamp = new SmartLamp
        {
            CompanyItIsAssociatedTo = companyOfDevice,
            DeviceName = "SmartLampName",
            DeviceModel = "SmartLampModel",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            IsTurnedOn = true
        };

        _repository.Add(newSmartLamp);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var smartLamps = otherDbContext.SmartLamps.ToList();

        smartLamps.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetById_WithValidId_ShouldReturnCorrectSmartLamp()
    {
        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var smartLampId = Guid.NewGuid();
        var smartLamp = new SmartLamp
        {
            Id = smartLampId,
            DeviceName = "Test SmartLamp",
            DeviceModel = "Model123",
            Description = "Test Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            CompanyId = companyId,
            IsTurnedOn = false
        };

        var dbContext = DbContextBuilder.BuildTestDbContext();
        dbContext.Companies.Add(company);
        dbContext.Devices.Add(smartLamp);
        dbContext.SaveChanges();

        var result = _repository.GetSmartLampByHardwareId(smartLampId);

        result.Should().NotBeNull();
        result.Id.Should().Be(smartLampId);
        result.DeviceName.Should().Be(smartLamp.DeviceName);
        result.DeviceModel.Should().Be(smartLamp.DeviceModel);
        result.Description.Should().Be(smartLamp.Description);
        result.Photos.Should().BeEquivalentTo(smartLamp.Photos);
        result.DeviceType.Should().Be(DeviceType.SmartLamp);
        result.IsTurnedOn.Should().Be(false);
    }

    [TestMethod]
    public void Update_WithValidSmartLamp_ShouldUpdateCorrectly()
    {
        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var smartLampId = Guid.NewGuid();
        var smartLamp = new SmartLamp
        {
            Id = smartLampId,
            DeviceName = "Test SmartLamp",
            DeviceModel = "Model123",
            Description = "Test Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            CompanyId = companyId,
            IsTurnedOn = false
        };

        var newSmartLamp = new SmartLamp
        {
            Id = smartLampId,
            DeviceName = "Test SmartLamp",
            DeviceModel = "Model123",
            Description = "Test Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            CompanyId = companyId,
            IsTurnedOn = true
        };

        var dbContext = DbContextBuilder.BuildTestDbContext();
        dbContext.Companies.Add(company);
        dbContext.Devices.Add(smartLamp);
        dbContext.SaveChanges();

        var result = _repository.Update(newSmartLamp);

        result.Should().NotBeNull();
        result.Id.Should().Be(smartLampId);
        result.DeviceName.Should().Be(smartLamp.DeviceName);
        result.DeviceModel.Should().Be(smartLamp.DeviceModel);
        result.Description.Should().Be(smartLamp.Description);
        result.Photos.Should().BeEquivalentTo(smartLamp.Photos);
        result.DeviceType.Should().Be(DeviceType.SmartLamp);
        result.IsTurnedOn.Should().Be(true);
    }

    [TestMethod]
    public void GetById_WithInvalidId_ShouldReturnNull()
    {
        var result = _repository.GetSmartLampByHardwareId(Guid.NewGuid());
        result.Should().BeNull();
    }
}
