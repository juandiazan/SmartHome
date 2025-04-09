using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using ImporterService;
using ModeloValidador.Abstracciones;
using Moq;
using PaginationAndFilters.Models;

namespace BusinessLogic.Test;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private Mock<IModelValidatorAdapter> _modelValidator = null!;
    private Mock<IAssemblyLoadingService<IDeviceImporter>> _deviceImporter = null!;
    private Mock<ICameraService> _cameraService = null!;
    private Mock<ISmartLampService> _smartLampService = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<ICompanyService> _companyService = null!;
    private Mock<IPathValidator> _pathValidator = null!;
    private DeviceService _deviceService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _modelValidator = new Mock<IModelValidatorAdapter>(MockBehavior.Strict);
        _deviceImporter = new Mock<IAssemblyLoadingService<IDeviceImporter>>(MockBehavior.Strict);
        _cameraService = new Mock<ICameraService>(MockBehavior.Strict);
        _smartLampService = new Mock<ISmartLampService>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _pathValidator = new Mock<IPathValidator>(MockBehavior.Strict);
        _deviceService = new DeviceService(
            _deviceRepository.Object,
            _modelValidator.Object,
            _deviceImporter.Object,
            _cameraService.Object,
            _smartLampService.Object,
            _sessionService.Object,
            _companyService.Object,
            _pathValidator.Object);
    }

    #region Create
    [DataRow("")]
    [DataRow(null)]
    [TestMethod]
    public void CreateSensor_WithNullName_ShouldThrowSensorException(string name)
    {
        var act = () => new CreateDeviceArgs(name, "Model123", "Description", ["photo1", "photo2"], "Sensor");

        act.Should().Throw<ArgumentNullException>("Sensor name cannot be empty");
    }

    [DataRow("")]
    [DataRow(null)]
    [TestMethod]
    public void CreateSensor_WithNullModel_ShouldThrowSensorException(string model)
    {
        var act = () => new CreateDeviceArgs("SensorName", model, "Description", ["photo1", "photo2"], "Sensor");

        act.Should().Throw<ArgumentNullException>("Sensor model cannot be empty");
    }

    [DataRow("")]
    [DataRow(null)]
    [TestMethod]
    public void CreateSensor_WithNullDescription_ShouldThrowSensorException(string description)
    {
        var act = () => new CreateDeviceArgs("SensorName", "Model123", description, ["photo1", "photo2"], "Sensor");

        act.Should().Throw<ArgumentNullException>("Device description cannot be empty");
    }

    [TestMethod]
    public void CreateSensor_WithNullPhotos_ShouldThrowSensorException()
    {
        var act = () => new CreateDeviceArgs("SensorName", "Model123", "Description", null, "Sensor");

        act.Should().Throw<ArgumentNullException>("Device photos cannot be null or empty");
    }

    [TestMethod]
    public void SensorDeviceType_IfItsNotSensor_ShouldThrowSensorException()
    {
        // Arrange
        var token = "token";
        var newSensor = new CreateDeviceArgs("SensorName", "Model123", "Description", ["photo1", "photo2"], "Camera");

        // Act
        Action act = () => _deviceService.Create(newSensor, token);

        // Assert
        act.Should().Throw<ArgumentException>("Device type must be sensor");
    }

    [TestMethod]
    public void CreateSensor_WithNullModelValidator_ShouldThrowKeyNotFoundException()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newDevice = new CreateDeviceArgs("SensorName", "Model123", "Description", ["photo1", "photo2"], "Sensor");

        var modelValidator = new Mock<IModeloValidador>(MockBehavior.Strict);

        var returnedUser = new User
        {
            Id = userId,
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };

        var company = new Company
        {
            Id = companyId,
            DeviceModelValidatorId = modelValidatorId
        };

        _sessionService
            .Setup(ss => ss.GetUserByToken(token))
            .Returns(returnedUser);

        _companyService
            .Setup(cs => cs.GetCompanyByOwnerId(userId))
            .Returns(company);

        _modelValidator
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, It.IsAny<string>()))
            .Throws<KeyNotFoundException>();

        var act = () => _deviceService.Create(newDevice, token);

        act.Should().Throw<KeyNotFoundException>("Model validator could not be found");
    }

    [TestMethod]
    public void CreateSensor_WithValidData_ShouldReturnSensor()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newDevice = new CreateDeviceArgs("SensorName", "Model123", "Description", ["photo1", "photo2"], "Sensor");

        var returnedUser = new User
        {
            Id = userId,
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };

        var company = new Company
        {
            Id = companyId,
            DeviceModelValidatorId = modelValidatorId
        };

        _sessionService
            .Setup(ss => ss.GetUserByToken(token))
            .Returns(returnedUser);

        _companyService
            .Setup(cs => cs.GetCompanyByOwnerId(userId))
            .Returns(company);

        _modelValidator
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, newDevice.DeviceModel))
            .Returns(true);

        _deviceRepository
            .Setup(act => act.Add(It.IsAny<Device>()))
            .Returns(new Device());

        var result = _deviceService.Create(newDevice, token);

        result.Should().NotBeNull();

        result.Id.Should().NotBeEmpty();

        result.CompanyId.Should().Be(companyId);
        result.DeviceName.Should().Be(newDevice.DeviceName);
        result.DeviceModel.Should().Be(newDevice.DeviceModel);
        result.Description.Should().Be(newDevice.Description);
        result.Photos.Should().BeEquivalentTo(newDevice.Photos);
        result.DeviceType.Should().Be(Enum.Parse<DeviceType>(newDevice.DeviceType));
    }

    [TestMethod]
    public void CreateMovementSensor_WithValidData_ShouldReturnSensor()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newDevice = new CreateDeviceArgs("SensorName", "Model123", "Description", ["photo1", "photo2"], "MovementSensor");

        var returnedUser = new User
        {
            Id = userId,
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };

        var company = new Company
        {
            Id = companyId,
            DeviceModelValidatorId = modelValidatorId
        };

        _sessionService
            .Setup(ss => ss.GetUserByToken(token))
            .Returns(returnedUser);

        _companyService
            .Setup(cs => cs.GetCompanyByOwnerId(userId))
            .Returns(company);

        _modelValidator
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, newDevice.DeviceModel))
            .Returns(true);

        _deviceRepository
            .Setup(act => act.Add(It.IsAny<Device>()))
            .Returns(new Device());

        var result = _deviceService.Create(newDevice, token);

        result.Should().NotBeNull();

        result.Id.Should().NotBeEmpty();

        result.CompanyId.Should().Be(companyId);
        result.DeviceName.Should().Be(newDevice.DeviceName);
        result.DeviceModel.Should().Be(newDevice.DeviceModel);
        result.Description.Should().Be(newDevice.Description);
        result.Photos.Should().BeEquivalentTo(newDevice.Photos);
        result.DeviceType.Should().Be(Enum.Parse<DeviceType>(newDevice.DeviceType));
    }

    #endregion

    #region List - Pagination - Filters
    [TestMethod]
    public void List_AllDeviceTypes_ShouldListCorrectly()
    {
        var deviceTypes = _deviceService.GetAllDeviceTypes();

        deviceTypes.Contains(DeviceType.Camera.ToString()).Should().BeTrue();
        deviceTypes.Contains(DeviceType.Sensor.ToString()).Should().BeTrue();
    }

    [TestMethod]
    public void GetAll_WithCorrectData_ShouldReturnCorrectElements()
    {
        var id = Guid.NewGuid().ToString();
        var deviceDto = new GetAllDevicesArgs(id, "Sensor1", "Model1", "photo1", "Company1", "Sensor");

        var resultantDevices = new List<GetAllDevicesArgs>
        {
            deviceDto
        };

        var device = new Device
        {
            Id = Guid.Parse(id),
            DeviceName = "Sensor1",
            DeviceModel = "Model1",
            Photos = ["photo1"],
            Description = "Description",
            DeviceType = DeviceType.Sensor,
            CompanyItIsAssociatedTo = new Company { CompanyName = "Company1" }
        };

        _deviceRepository
            .Setup(act => act.GetAll(new DeviceFilterArgs()))
            .Returns([device]);

        var resultantList = _deviceService.GetAll(new DeviceFilterArgs());

        resultantList.Should().BeEquivalentTo(resultantDevices);
    }

    [TestMethod]
    [DataRow(0, 2)]
    [DataRow(-1, 2)]
    [DataRow(2, 0)]
    [DataRow(2, -1)]
    public void GetAll_WithNegativeOrZeroForPagination_ShouldThrowException(int page, int pageSize)
    {
        var id = Guid.NewGuid().ToString();
        var devicesArgs = new List<GetAllDevicesArgs>
        {
            new(id, (string)"Camera1", (string)"Model3", (string)"photo3", (string)"Company3", (string)"camera")
        };

        var act = () => _deviceService.GetAll(new DeviceFilterArgs(page, pageSize));

        act.Should().Throw<FormatException>("Current page and page size cannot be negative or zero");
    }
    #endregion

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WithInvalidImplementation_ShouldThrowFormatException()
    {
        var args = new ImportDevicesArgs
        {
            DeviceImporterImplementationId = "invalid Id",
            Path = "path"
        };

        var act = () => _deviceService.ImportDevices(args, It.IsAny<string>());

        act.Should().Throw<FormatException>("Device importer implementation could not be found");
    }

    [TestMethod]
    public void ImportDevices_WithInvalidPath_ShouldThrowKeyNotFoundException()
    {
        var args = new ImportDevicesArgs
        {
            DeviceImporterImplementationId = Guid.NewGuid().ToString(),
            Path = "invalid path"
        };

        _pathValidator
            .Setup(service => service.PathExists(args.Path))
            .Returns(false);

        var act = () => _deviceService.ImportDevices(args, It.IsAny<string>());

        act.Should().Throw<KeyNotFoundException>("File not found");
    }

    [TestMethod]
    public void ImportDevices_WithCorrectData_ShouldCreate()
    {
        var sessionToken = "token";
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var validatorId = Guid.NewGuid();
        var args = new ImportDevicesArgs
        {
            DeviceImporterImplementationId = Guid.NewGuid().ToString(),
            Path = "valid path"
        };
        var companyOwner = new CompanyOwner
        {
            Id = Guid.NewGuid(),
            Name = "Owner",
            Surname = "Surname",
            Email = "email",
            Password = "password",
            AssociatedCompany = new Company { Id = companyId, DeviceModelValidatorId = validatorId }
        };

        var modelValidator = new Mock<IModeloValidador>(MockBehavior.Strict);

        _pathValidator
            .Setup(service => service.PathExists(args.Path))
            .Returns(true);

        _sessionService
            .Setup(service => service.GetUserByToken(sessionToken))
            .Returns(companyOwner);

        _companyService
            .Setup(service => service.GetCompanyByOwnerId(companyOwner.Id))
            .Returns(companyOwner.AssociatedCompany);

        _deviceImporter
            .Setup(service => service.LoadImplementations());

        var testImporter = Activator.CreateInstance(typeof(TestDeviceImporter)) as IDeviceImporter;
        _deviceImporter
            .Setup(service => service.GetImplementationById(Guid.Parse(args.DeviceImporterImplementationId)))
            .Returns(testImporter!);

        _modelValidator
            .Setup(act => act.IsDeviceModelValid(validatorId, It.IsAny<string>()))
            .Returns(true);

        _deviceRepository
            .Setup(act => act.Add(It.IsAny<Device>()))
            .Returns(It.IsAny<Device>());

        _deviceService.ImportDevices(args, sessionToken);

        _sessionService.VerifyAll();
        _companyService.VerifyAll();
        _deviceImporter.VerifyAll();
        _pathValidator.VerifyAll();
    }

    #endregion
}
