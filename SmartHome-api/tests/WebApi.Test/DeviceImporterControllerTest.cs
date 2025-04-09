using FluentAssertions;
using IBusinessLogic;
using ImporterService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Test;

[TestClass]
public class DeviceImporterControllerTest
{
    [TestMethod]
    public void GetAllDeviceImporters_ReturnsOk()
    {
        var assemblyLoadingService = new Mock<IAssemblyLoadingService<IDeviceImporter>>();
        var controller = new DeviceImporterController(assemblyLoadingService.Object);

        var result = controller.GetDeviceImporters();

        result.Should().BeOfType<OkObjectResult>();
    }
}
