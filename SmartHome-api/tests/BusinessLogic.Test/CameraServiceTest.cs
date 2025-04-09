using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using ModeloValidador.Abstracciones;
using Moq;
namespace BusinessLogic.Test;

[TestClass]
public class CameraServiceTest
{
    private Mock<IAddRepository<Camera>> _cameraRepository = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<ICompanyService> _companyService = null!;
    private Mock<IModelValidatorAdapter> _modelValidator = null!;
    private CameraService _cameraService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _cameraRepository = new Mock<IAddRepository<Camera>>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _modelValidator = new Mock<IModelValidatorAdapter>(MockBehavior.Strict);

        _cameraService = new CameraService(
            _cameraRepository.Object,
            _sessionService.Object,
            _companyService.Object,
            _modelValidator.Object);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void CreateSecurityCamera_WithNullName_ShouldThrowCameraException(string name)
    {
        var act = () => new CreateCameraArgs(name, "CameraModel", "CameraDescription", ["photo1", "photo2"], "Camera",
            true, false, false, false);

        act.Should().Throw<ArgumentNullException>("Camera name cannot be null or empty");
    }

    [DataRow("")]
    [DataRow(null)]
    [TestMethod]
    public void CreateSecurityCamera_WithNullModel_ShouldThrowCameraException(string model)
    {
        var act = () => new CreateCameraArgs("CameraName", model, "CameraDescription", ["photo1", "photo2"], "Camera",
            true, false, false, false);

        act.Should().Throw<ArgumentNullException>("Camera model cannot be empty");
    }

    [DataRow("")]
    [DataRow(null)]
    [TestMethod]
    public void CreateSecurityCamera_WithNullDescription_ShouldThrowCameraException(string description)
    {
        var act = () => new CreateCameraArgs("CameraName", "CameraModel", description, ["photo1", "photo2"], "Camera",
            true, false, false, false);

        act.Should().Throw<ArgumentNullException>("Device description cannot be empty");
    }

    [TestMethod]
    public void CreateSecurityCamera_WithNullPhotos_ShouldThrowCameraException()
    {
        var act = () => new CreateCameraArgs("CameraName", "CameraModel", "CameraDescription", null, "Camera", true,
            false, false, false);

        act.Should().Throw<ArgumentNullException>("Camera photos cannot be null");
    }

    [TestMethod]
    public void CreateCamera_WithNullModelValidator_ShouldThrowKeyNotFoundException()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newCamera = new CreateCameraArgs(
            "CameraName",
            "CameraModel",
            "CameraDescription",
            ["photo1", "photo2"],
            "Camera",
            true,
            false,
            false,
            false);

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

        var act = () => _cameraService.Create(newCamera, token);

        act.Should().Throw<KeyNotFoundException>("Model validator could not be found");
    }

    [TestMethod]
    public void CameraDeviceType_IfItsNotSecurityCamera_ShouldThrowCameraException()
    {
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var token = "token";
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

        var newCamera = new CreateCameraArgs("CameraName", "CameraModel", "CameraDescription", ["photo1", "photo2"],
            "Sensor", true, false, false, false);

        Action act = () => _cameraService.Create(newCamera, token);

        act.Should().Throw<ArgumentException>("Device type must be camera");
    }

    [TestMethod]
    public void CreateCamera_WithValidData_ShouldReturnCamera()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newCamera = new CreateCameraArgs(
            "CameraName",
            "CameraModel",
            "CameraDescription",
            ["photo1", "photo2"],
            "Camera",
            true,
            false,
            false,
            false);

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
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, newCamera.DeviceModel))
            .Returns(true);

        _cameraRepository
            .Setup(act => act.Add(It.IsAny<Camera>()))
            .Returns(new Camera());

        var result = _cameraService.Create(newCamera, token);

        result.CompanyId.Should().Be(companyId);
        result.DeviceName.Should().Be(newCamera.DeviceName);
        result.DeviceModel.Should().Be(newCamera.DeviceModel);
        result.Description.Should().Be(newCamera.Description);
        result.Photos.Should().BeEquivalentTo(newCamera.Photos);
        result.DeviceType.Should().Be(Enum.Parse<DeviceType>(newCamera.DeviceType));
        result.CanBeUsedIndoors.Should().Be(newCamera.CanBeUsedIndoors);
        result.CanBeUsedOutdoors.Should().Be(newCamera.CanBeUsedOutdoors);
    }

    [TestMethod]
    public void CreateCamera_WithMovementDetectionSupportAndPersonDetectionSupport_ShouldReturnCamera()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newCamera = new CreateCameraArgs("CameraName", "CameraModel", "CameraDescription", ["photo1", "photo2"],
            "Camera", true, false, true, true);

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
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, newCamera.DeviceModel))
            .Returns(true);

        _cameraRepository
            .Setup(act => act.Add(It.IsAny<Camera>()))
            .Returns(new Camera());

        var result = _cameraService.Create(newCamera, token);

        result.CompanyId.Should().Be(companyId);
        result.DeviceName.Should().Be(newCamera.DeviceName);
        result.DeviceModel.Should().Be(newCamera.DeviceModel);
        result.Description.Should().Be(newCamera.Description);
        result.Photos.Should().BeEquivalentTo(newCamera.Photos);
        result.DeviceType.Should().Be(Enum.Parse<DeviceType>(newCamera.DeviceType));
        result.CanBeUsedIndoors.Should().Be(newCamera.CanBeUsedIndoors);
        result.CanBeUsedOutdoors.Should().Be(newCamera.CanBeUsedOutdoors);
    }
}
