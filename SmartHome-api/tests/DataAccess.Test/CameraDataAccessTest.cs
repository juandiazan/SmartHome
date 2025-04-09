using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using IDataAccess;

namespace DataAccess.Test;

[TestClass]
public class CameraDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly IAddRepository<Camera> _repository;

    public CameraDataAccessTest()
    {
        _repository = new CameraRepository(_dbContext);
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
    public void Add_WithCorrectCamera_ShouldAddCorrectly()
    {
        var companyOfDevice = new Company
        {
            CompanyName = "Name",
            Logotype = "Logotype",
            Rut = "Rut",
        };

        var newCamera = new Camera
        {
            CompanyItIsAssociatedTo = companyOfDevice,
            DeviceName = "CameraName",
            DeviceModel = "CameraModel",
            Description = "Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true
        };

        _repository.Add(newCamera);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var cameras = otherDbContext.Cameras.ToList();

        cameras.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetById_WithValidId_ShouldReturnCorrectCamera()
    {
        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var cameraId = Guid.NewGuid();
        var camera = new Camera
        {
            Id = cameraId,
            DeviceName = "Test Camera",
            DeviceModel = "Model123",
            Description = "Test Description",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.Camera,
            HasMovementDetectionSupport = true,
            HasPersonDetectionSupport = true,
            CanBeUsedIndoors = true,
            CanBeUsedOutdoors = true,
            CompanyId = companyId
        };

        var dbContext = DbContextBuilder.BuildTestDbContext();
        dbContext.Companies.Add(company);
        dbContext.Devices.Add(camera);
        dbContext.SaveChanges();

        var result = _dbContext.Cameras.FirstOrDefault(c => c.Id == cameraId)!;

        result.Should().NotBeNull();
        result.Id.Should().Be(cameraId);
        result.DeviceName.Should().Be(camera.DeviceName);
        result.DeviceModel.Should().Be(camera.DeviceModel);
        result.Description.Should().Be(camera.Description);
        result.Photos.Should().BeEquivalentTo(camera.Photos);
        result.DeviceType.Should().Be(DeviceType.Camera);
        result.HasMovementDetectionSupport.Should().Be(camera.HasMovementDetectionSupport);
        result.HasPersonDetectionSupport.Should().Be(camera.HasPersonDetectionSupport);
        result.CanBeUsedIndoors.Should().Be(camera.CanBeUsedIndoors);
        result.CanBeUsedOutdoors.Should().Be(camera.CanBeUsedOutdoors);
    }
}
