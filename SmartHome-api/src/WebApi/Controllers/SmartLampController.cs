using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("smart-lamps")]
public sealed class SmartLampController(ISmartLampService smartLampService) : ControllerBase
{
    [HttpPost("{hardwareId}/connection-state")]

    public IActionResult ChangeState(Guid hardwareId)
    {
        var state = smartLampService.ChangeState(hardwareId);
        var message = "Smart lamp turned";
        return Ok(state ? message + " on." : message + " off.");
    }

    [HttpPost]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult CreateSmartLamp([FromBody] CreateSmartLampRequest request, [FromHeader] string authorization)
    {
        var smartLamp = smartLampService.Create(request.ToArgs(), authorization);
        return Created(
            $"smart-lamps/{smartLamp.Id}",
            new
            {
                message = "SmartLamp created successfully",
                CreatedSmartLamp = new CreateSmartLampResponse(
                    smartLamp.DeviceName,
                    smartLamp.DeviceModel,
                    smartLamp.Description,
                    smartLamp.Photos[0],
                    smartLamp.DeviceType.ToString(),
                    smartLamp.IsTurnedOn)
            });
    }
}
