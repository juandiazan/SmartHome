using IBusinessLogic;
using ImporterService;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("importers")]
public class DeviceImporterController(IAssemblyLoadingService<IDeviceImporter> assemblyLoadingService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult GetDeviceImporters()
    {
        return Ok(assemblyLoadingService.GetImplementations());
    }
}
