using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using ModeloValidador.Abstracciones;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class SmartLampServiceTest
{
    private Mock<ISmartLampRepository> _smartLampRepository = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<ICompanyService> _companyService = null!;
    private Mock<IModelValidatorAdapter> _modelValidator = null!;
    private SmartLampService _smartLampService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _smartLampRepository = new Mock<ISmartLampRepository>(MockBehavior.Strict);
        _companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _modelValidator = new Mock<IModelValidatorAdapter>(MockBehavior.Strict);

        _smartLampService = new SmartLampService(_smartLampRepository.Object, _sessionService.Object, _companyService.Object, _modelValidator.Object);
    }

    [DataRow(null)]
    [DataRow("")]
    [TestMethod]
    public void CreateSmartLamp_WithEmptyOrNullName_ShouldThrowArgumentException(string name)
    {
        var args = () => new CreateSmartLampArgs(name, "LampModel", "LampDescription", ["photo1", "photo2"], true, DeviceType.SmartLamp.ToString());

        args.Should().Throw<ArgumentNullException>().WithMessage("Device name cannot be empty");
    }

    [DataRow(null)]
    [DataRow("")]
    [TestMethod]
    public void CreateSmartLamp_WithEmptyOrNullModel_ShouldThrowArgumentException(string model)
    {
        var args = () => new CreateSmartLampArgs("LampName", model, "LampDescription", ["photo1", "photo2"], true, DeviceType.SmartLamp.ToString());

        args.Should().Throw<ArgumentNullException>().WithMessage("Device model cannot be empty");
    }

    [DataRow(null)]
    [DataRow("")]
    [TestMethod]
    public void CreateSmartLamp_WithEmptyOrNullDescription_ShouldThrowArgumentException(string description)
    {
        var args = () => new CreateSmartLampArgs("LampName", "LampModel", description, ["photo1", "photo2"], true, DeviceType.SmartLamp.ToString());

        args.Should().Throw<ArgumentNullException>().WithMessage("Device description cannot be empty");
    }

    [TestMethod]
    public void CreateSmartLamp_WhitNullPhotos_ShouldThrowArgumentException()
    {
        var args = () => new CreateSmartLampArgs("LampName", "LampModel", "LampDescription", null, true, DeviceType.SmartLamp.ToString());

        args.Should().Throw<ArgumentNullException>().WithMessage("Device photos cannot be null or empty");
    }

    [TestMethod]
    public void CreateSmartLamp_IfNotSmartLampType_ShouldThrowArgumentException()
    {
        // Arrange
        var token = "token";
        var companyId = Guid.NewGuid().ToString();
        var args = new CreateSmartLampArgs("LampName", "LampModel", "LampDescription", ["photo1", "photo2"], true, string.Empty);

        // Act
        Action act = () => _smartLampService.Create(args, token);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device type must be SmartLamp");
    }

    [TestMethod]
    public void CreateSmartLamp_WithCorrectData_ShouldCreateCorrectly()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var newSmartLamp = new CreateSmartLampArgs("LampName", "LampModel", "LampDescription", ["photo1", "photo2"], true, DeviceType.SmartLamp.ToString());

        var returnedSmartLamp = new SmartLamp
        {
            DeviceName = newSmartLamp.DeviceName,
            DeviceModel = newSmartLamp.DeviceModel,
            Description = newSmartLamp.Description,
            Photos = newSmartLamp.Photos,
            DeviceType = DeviceType.SmartLamp,
            CompanyId = companyId,
            IsTurnedOn = newSmartLamp.IsTurnedOn
        };

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
            .Setup(act => act.IsDeviceModelValid(modelValidatorId, newSmartLamp.DeviceModel))
            .Returns(true);

        _smartLampRepository
            .Setup(repo => repo.Add(It.IsAny<SmartLamp>()))
            .Returns(returnedSmartLamp);

        var result = _smartLampService.Create(newSmartLamp, token);

        result.DeviceName.Should().Be(newSmartLamp.DeviceName);
        result.DeviceModel.Should().Be(newSmartLamp.DeviceModel);
        result.Description.Should().Be(newSmartLamp.Description);
        result.Photos.Should().BeEquivalentTo(newSmartLamp.Photos);
        result.DeviceType.Should().Be(DeviceType.SmartLamp);
        result.IsTurnedOn.Should().Be(newSmartLamp.IsTurnedOn);
    }

    [TestMethod]
    public void ChangeState_WithCorrectData_ShouldTurnOnLampStatus()
    {
        var companyId = Guid.NewGuid().ToString();
        var smartLampId = Guid.NewGuid();

        var smartLamp = new SmartLamp
        {
            Id = smartLampId,
            DeviceName = "LampName",
            DeviceModel = "LampModel",
            Description = "LampDescription",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            CompanyId = Guid.Parse(companyId),
            IsTurnedOn = false
        };

        var smartLampTurnedOn = new SmartLamp
        {
            Id = smartLampId,
            DeviceName = "LampName",
            DeviceModel = "LampModel",
            Description = "LampDescription",
            Photos = ["photo1", "photo2"],
            DeviceType = DeviceType.SmartLamp,
            CompanyId = Guid.Parse(companyId),
            IsTurnedOn = true
        };

        _smartLampRepository
            .Setup(repo => repo.GetSmartLampByHardwareId(smartLampId))
            .Returns(smartLamp);

        _smartLampRepository
            .Setup(repo => repo.Update(smartLamp))
            .Returns(smartLampTurnedOn);

        var isTurnedOn = _smartLampService.ChangeState(smartLamp.Id);

        isTurnedOn.Should().BeTrue();
    }

    [TestMethod]
    public void CreateSmartLamp_WithNullModelValidator_ShouldThrowKeyNotFoundException()
    {
        var token = "token";
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var newSmartLamp = new CreateSmartLampArgs("LampName", "LampModel", "LampDescription", ["photo1", "photo2"], true, DeviceType.SmartLamp.ToString());

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

        var act = () => _smartLampService.Create(newSmartLamp, token);

        act.Should().Throw<KeyNotFoundException>("Model validator could not be found");
    }
}
