using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;

namespace WebApi.Controllers;

[ApiController]
[Route("rooms")]
public class RoomController(IHomeService homeService) : ControllerBase
{
    [HttpPut]
    [Route("{roomId}/devices")]
    [CustomAuthorizeFilter("home-owner")]
    public IActionResult AddHomeDeviceToRoom(Guid roomId, [FromBody] AddDeviceToRoomRequest newDevice)
    {
        homeService.AddDeviceToRoomOfHome(roomId, newDevice.HardwareId);
        return Ok("Home device successfully added to room");
    }
}
