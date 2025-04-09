using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using ModeloValidador.Abstracciones;
using Moq;
using WebApi.Controllers;

namespace WebApi.Test;

[TestClass]
public class ModelValidatorControllerTest
{
    [TestMethod]
    public void GetAllModelValidators_ReturnsOk()
    {
        var assemblyLoadingService = new Mock<IAssemblyLoadingService<IModeloValidador>>();
        var controller = new ModelValidatorController(assemblyLoadingService.Object);

        var result = controller.GetAllModelValidators();

        result.Should().BeOfType<OkObjectResult>();
    }
}
