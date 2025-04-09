using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;

namespace WebApi.Controllers;

[ApiController]
[Route("home-devices")]
public class HomeDeviceController(IHomeDeviceService homeDeviceService) : ControllerBase
{
    [HttpPut("{hardwareId}/alias")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult UpdateHomeDeviceAlias([FromRoute] string hardwareId, [FromBody] UpdateHomeDeviceAliasRequest newHomeDeviceAlias)
    {
        newHomeDeviceAlias.HardwareId = hardwareId;
        var homeDevice = homeDeviceService.UpdateHomeDeviceAlias(newHomeDeviceAlias.ToArgs());
        return Ok($"Alias modified successfully. New alias is {homeDevice.Alias}");
    }

    [HttpPut("{hardwareId}/connection-state")]
    public IActionResult UpdateHomeDeviceConnectionState([FromRoute] string hardwareId)
    {
        var result = homeDeviceService.UpdateHomeDeviceConnectionState(hardwareId);
        return Ok("Device has been turned" + (result ? " on" : " off"));
    }
}
