using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using ModeloValidador.Abstracciones;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("model-validators")]
public sealed class ModelValidatorController(IAssemblyLoadingService<IModeloValidador> assemblyLoadingService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult GetAllModelValidators()
    {
        return Ok(assemblyLoadingService.GetImplementations());
    }
}
