using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Queries;
using WebApi.Models.Requests;

namespace WebApi.Controllers;

[ApiController]
[Route("devices")]
public class DeviceController(IDeviceService deviceService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult ImportDevices([FromBody] ImportDevicesRequest request, [FromHeader] string authorization)
    {
        deviceService.ImportDevices(request.ToArgs(), authorization);
        return Ok("Devices imported successfully.");
    }

    [HttpGet]
    [CustomAuthorizeFilter]
    public IActionResult GetAllDevices([FromQuery] GetDevicesQuery query)
    {
        return Ok(deviceService.GetAll(query.ToArgs()));
    }
}
