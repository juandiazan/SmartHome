using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("device-types")]
public class DeviceTypesController(IDeviceService deviceTypeService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter]
    [ResponseCache(Duration = 1200, Location = ResponseCacheLocation.Client)]
    public IActionResult GetAllDeviceTypes()
    {
        return Ok(deviceTypeService.GetAllDeviceTypes());
    }
}
