using System.Net;
using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaginationAndFilters.Models;
using WebApi.Controllers;
using WebApi.Models.Queries;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class DeviceControllerTest
{
    private Mock<IDeviceService> _sensorService = null!;
    private Mock<ICameraService> _cameraService = null!;
    private Mock<ISmartLampService> _smartLampService = null!;

    private Mock<INotificationService> _notificationService = null!;

    private SensorController _sensorController = null!;
    private CameraController _cameraController = null!;
    private SmartLampController _smartLampController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sensorService = new Mock<IDeviceService>(MockBehavior.Strict);
        _cameraService = new Mock<ICameraService>(MockBehavior.Strict);
        _smartLampService = new Mock<ISmartLampService>(MockBehavior.Strict);

        _notificationService = new Mock<INotificationService>(MockBehavior.Strict);

        _sensorController = new SensorController(_sensorService.Object, _notificationService.Object);
        _cameraController = new CameraController(_cameraService.Object, _notificationService.Object);
        _smartLampController = new SmartLampController(_smartLampService.Object);
    }

    [TestMethod]
    public void CreateWindowOpenedNotification_WithCorrectData_ShouldCreateCorrectly()
    {
        var triggeringDeviceId = Guid.NewGuid();
        var deviceType = DeviceType.Sensor;

        _notificationService
            .Setup(service => service.GenerateAndSendNotification(new NotificationGenerationArgs(triggeringDeviceId, deviceType.ToString(), "window-opened", null)))
            .Verifiable();

        var result = _sensorController.CreateWindowOpenedNotification(triggeringDeviceId);

        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void CreateWindowOpenedNotification_WithNullDeviceGuid_ShouldBadRequest()
    {
        var triggeringDeviceId = Guid.Empty;
        var deviceType = DeviceType.Sensor;
        var args = new NotificationGenerationArgs(triggeringDeviceId, deviceType.ToString(), "window-opened", null);
        _notificationService
            .Setup(service => service.GenerateAndSendNotification(args))
            .Throws(new InvalidOperationException("Device is not online or does not exist."));

        Action act = () => _sensorController.CreateWindowOpenedNotification(triggeringDeviceId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not online or does not exist.");
    }

    [TestMethod]
    public void CreateWindowClosedNotification_WithCorrectData_ShouldCreateCorrectly()
    {
        var triggeringDeviceId = Guid.Empty;
        var deviceType = DeviceType.Sensor;
        var args = new NotificationGenerationArgs(triggeringDeviceId, deviceType.ToString(), "window-closed", null);

        _notificationService
            .Setup(service => service.GenerateAndSendNotification(args))
            .Verifiable();

        var result = _sensorController.CreateWindowClosedNotification(triggeringDeviceId);

        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void CreateWindowClosedNotification_WithNullDeviceGuid_ShouldBadRequest()
    {
        var triggeringDeviceId = Guid.Empty;
        var deviceType = DeviceType.Sensor;
        var args = new NotificationGenerationArgs(triggeringDeviceId, deviceType.ToString(), "window-closed", null);

        _notificationService
            .Setup(service => service.GenerateAndSendNotification(args))
            .Throws(new InvalidOperationException("Device is not online or does not exist."));

        Action act = () => _sensorController.CreateWindowClosedNotification(triggeringDeviceId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not online or does not exist.");
    }

    [TestMethod]
    public void CreateSecurityCamera_WithCorrectData_ShouldCreate()
    {
        var token = "token";
        var deviceId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();

        var user = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = new Role { RoleName = "company-owner" }
        };

        var request = new CreateCameraRequest(
            "Name",
            "Model",
            "Brand",
            ["pic1", "pic2"],
            "Username",
            true,
            true,
            true,
            true);

        var args = request.ToArgs();

        var expectedCamera = new Camera
        {
            Id = deviceId,
            CompanyId = companyId,
            DeviceName = request.CameraName!,
            DeviceModel = request.CameraModel!,
            Description = request.Description!,
            Photos = request.Photos!,
            DeviceType = DeviceType.Camera,
            CanBeUsedIndoors = request.CanBeUsedIndoors,
            CanBeUsedOutdoors = request.CanBeUsedOutdoors,
            HasMovementDetectionSupport = request.HasMovementDetectionSupport,
            HasPersonDetectionSupport = request.HasPersonDetectionSupport
        };

        _cameraService
            .Setup(act => act.Create(args, token))
            .Returns(expectedCamera);

        var result = _cameraController.CreateSecurityCamera(request, token) as CreatedResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void CreateSensors_WithCorrectData_ShouldCreate()
    {
        var token = "token";
        var deviceId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();

        var user = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = new Role { RoleName = "company-owner" }
        };

        var request = new CreateDeviceRequest("Name", "Model", "Brand", ["pic1", "pic2"], "Sensor");

        var args = request.ToArgs();

        _sensorService
            .Setup(act => act.Create(args, token))
            .Returns(new Device
            {
                Id = deviceId,
                CompanyId = companyId,
                DeviceName = request.DeviceName!,
                DeviceModel = request.DeviceModel!,
                Description = request.Description!,
                Photos = request.Photos!,
                DeviceType = DeviceType.Sensor
            });

        var result = _sensorController.CreateSensor(request, token) as CreatedResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void ListAllDevices_WithCorrectData_ShouldList()
    {
        var id = Guid.NewGuid().ToString();
        var args = new GetAllDevicesArgs(id, "DeviceName", "DeviceModel", "./desktop/pic1", "Owner", "Sensor");
        var filterArgs = new DeviceFilterArgs(1, 10, null, null, null, null);
        var query = new GetDevicesQuery(1, 10, null, null, null, null);

        var deviceService = new Mock<IDeviceService>(MockBehavior.Strict);

        deviceService
            .Setup(act => act.GetAll(filterArgs))
            .Returns(
            [
                new GetAllDevicesArgs(id, "DeviceName", "DeviceModel", "./desktop/pic1", "Owner", "Sensor")
            ]);

        var deviceController = new DeviceController(deviceService.Object);
        var result = deviceController.GetAllDevices(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void ListAllDevices_WithNoData_ShouldList()
    {
        var deviceService = new Mock<IDeviceService>(MockBehavior.Strict);
        var args = new DeviceFilterArgs(1, 10, null, null, null, null);
        var query = new GetDevicesQuery(1, 10, null, null, null, null);

        deviceService
            .Setup(act => act.GetAll(args))
            .Returns([]);

        var deviceController = new DeviceController(deviceService.Object);
        var result = deviceController.GetAllDevices(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void GetAllDeviceTypes_ShouldReturnOk()
    {
        var deviceTypeService = new Mock<IDeviceService>(MockBehavior.Strict);

        var listaDeDeviceTypes = new List<string> { "Sensor", "Camera" };

        deviceTypeService
            .Setup(service => service.GetAllDeviceTypes())
            .Returns(listaDeDeviceTypes);

        var deviceTypesController = new DeviceTypesController(deviceTypeService.Object);
        var result = deviceTypesController.GetAllDeviceTypes() as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeOfType<List<string>>();
    }

    [TestMethod]
    public void DetectMovementSensorMotion_WithCorrectData_ShouldSendNotification()
    {
        var triggeringDeviceId = Guid.Empty;
        var deviceType = DeviceType.MovementSensor;
        var args = new NotificationGenerationArgs(triggeringDeviceId, deviceType.ToString(), "motion-detected", null);

        _notificationService
            .Setup(act => act.GenerateAndSendNotification(args))
            .Verifiable();

        var result = _sensorController.DetectMovementSensorMotion(triggeringDeviceId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo("Motion detected and notifications sent.");

        _cameraService.Verify();
    }

    [TestMethod]
    public void DetectPerson_WithCorrectData_ShouldSendNotification()
    {
        var deviceType = DeviceType.Camera;
        var cameraId = Guid.NewGuid();
        var identifiedPerson = "Person";
        var args = new NotificationGenerationArgs(cameraId, deviceType.ToString(), "person-detected", identifiedPerson);
        var request = new DetectPersonRequest(identifiedPerson);

        _notificationService
            .Setup(act => act.GenerateAndSendNotification(args))
            .Verifiable();

        var result = _cameraController.DetectPerson(request, cameraId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo("Person detected and notifications sent.");

        _cameraService.Verify();
    }

    [TestMethod]
    public void ChangeState_WithCorrectData_ShouldTurnOn()
    {
        var deviceId = Guid.NewGuid();

        _smartLampService
            .Setup(act => act.ChangeState(It.IsAny<Guid>()))
            .Returns(true)
            .Verifiable();

        var result = _smartLampController.ChangeState(deviceId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo("Smart lamp turned on.");

        _smartLampService.Verify(act => act.ChangeState(It.IsAny<Guid>()), Times.Once);
    }

    [TestMethod]
    public void CreateSmartLamp_WithCorrectData_ShouldCreate()
    {
        var token = "token";
        var deviceId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();

        var user = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = new Role { RoleName = "company-owner" }
        };

        var request = new CreateSmartLampRequest(
            "Name",
            "Model",
            "Brand",
            ["pic1", "pic2"],
            "SmartLamp");

        var args = request.ToArgs();

        var expectedSmartLamp = new SmartLamp
        {
            Id = deviceId,
            CompanyId = companyId,
            DeviceName = request.LampName!,
            DeviceModel = request.LampModel!,
            Description = request.Description!,
            Photos = request.Photos!,
            DeviceType = DeviceType.SmartLamp,
            IsTurnedOn = true
        };

        _smartLampService
            .Setup(act => act.Create(args, token))
            .Returns(expectedSmartLamp);

        var result = _smartLampController.CreateSmartLamp(request, token) as CreatedResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void CreateMovementSensor_WithCorrectData_ShouldCreate()
    {
        var token = "token";
        var deviceId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var modelValidatorId = Guid.NewGuid();

        var user = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = new Role { RoleName = "company-owner" }
        };

        var request = new CreateDeviceRequest("Name", "Model", "Brand", ["pic1", "pic2"], "MovementSensor");

        var args = request.ToArgs();

        _sensorService
            .Setup(act => act.Create(args, token))
            .Returns(new Device
            {
                Id = deviceId,
                CompanyId = companyId,
                DeviceName = request.DeviceName!,
                DeviceModel = request.DeviceModel!,
                Description = request.Description!,
                Photos = request.Photos!,
                DeviceType = DeviceType.Sensor
            });

        var result = _sensorController.CreateSensor(request, token) as CreatedResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void ImportDevices_WithCorrectData_ShouldOk()
    {
        var sessionToken = "token";
        var importerId = "deviceImporterId";
        var path = "filePath";
        var request = new ImportDevicesRequest(importerId, path);
        var args = request.ToArgs();

        var deviceController = new DeviceController(_sensorService.Object);

        _sensorService
            .Setup(act => act.ImportDevices(args, sessionToken))
            .Verifiable();

        var result = deviceController.ImportDevices(request, sessionToken);

        result.Should().BeOfType<OkObjectResult>();
    }
}
