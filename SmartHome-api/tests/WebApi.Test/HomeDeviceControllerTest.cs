using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class HomeDeviceControllerTest
{
    private Mock<IHomeDeviceService> _homeDeviceService = null!;
    private HomeDeviceController _homeDeviceController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeDeviceService = new Mock<IHomeDeviceService>();
        _homeDeviceController = new HomeDeviceController(_homeDeviceService.Object);
    }

    [TestMethod]
    public void UpdateAlias_ShouldReturnOk()
    {
        var hdId = Guid.NewGuid();
        var alias = "alias";
        var request = new UpdateHomeDeviceAliasRequest(hdId.ToString(), alias);
        var args = new UpdateHomeDeviceArgs(request.HardwareId!, request.Alias!);

        var updatedDevice = new HomeDevice
        {
            HardwareId = Guid.Parse(request.HardwareId!),
            Alias = request.Alias!
        };

        _homeDeviceService
            .Setup(act => act.UpdateHomeDeviceAlias(It.Is<UpdateHomeDeviceArgs>(
                a => a.HardwareId == args.HardwareId && a.NewAlias == args.NewAlias)))
            .Returns(updatedDevice);

        var result = _homeDeviceController.UpdateHomeDeviceAlias(hdId.ToString(), request);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void UpdateConnectionState_ShouldReturnOk()
    {
        var hdId = Guid.NewGuid();
        _homeDeviceService
            .Setup(act => act.UpdateHomeDeviceConnectionState(hdId.ToString()))
            .Returns(true);

        var result = _homeDeviceController.UpdateHomeDeviceConnectionState(hdId.ToString());

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }
}
