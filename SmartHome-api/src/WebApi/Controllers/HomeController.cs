using DTOs;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("homes")]
public class HomeController(IHomeService homeService, ISessionService sessionService, IHomeDeviceService homeDeviceService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult GetHomes([FromHeader] string authorization)
    {
        var homes = homeService.GetHomesThatLoggedInUserBelongsTo(authorization);
        return Ok(homes);
    }

    [HttpPost]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult CreateHome([FromBody] CreateHomeRequest request, [FromHeader] string authorization)
    {
        var home = homeService.Create(request.ToArgs(sessionService.GetUserByToken(authorization)!.Email));
        return Created(
            $"homes/{home.Id}",
            new
            {
                message = "Home created successfully",
                CreatedHome = new CreateHomeResponse(
                    home.OwnerEmail,
                    home.Address.MainStreet!,
                    home.Address.DoorNumber,
                    home.Location.Latitude!,
                    home.Location.Longitude!,
                    home.MaxAmountOfMembers,
                    home.Alias)
            });
    }

    [HttpGet]
    [Route("{homeId}/devices")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    [CustomPermissionFilter("list-devices-of-specific-home")]
    public IActionResult ListHomeDevices([FromRoute] Guid homeId, [FromQuery] string? room)
    {
        return Ok(homeService.ListHomeDevices(homeId, room));
    }

    [HttpPut]
    [Route("{homeId}/members")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult AddMemberToHome([FromRoute] Guid homeId, [FromBody] NewMemberToAddRequest memberRequest)
    {
        homeService.AddMemberToHome(homeId, memberRequest.ToArgs());
        return Ok("Member added successfully to the home");
    }

    [HttpPut]
    [Route("{homeId}/devices")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    [CustomPermissionFilter("add-device-to-specific-home")]

    public IActionResult AddDeviceToHome([FromRoute] Guid homeId, [FromBody] AddDeviceToHomeRequest args)
    {
        var hdArgs = new CreateHomeDeviceArgs(homeId, Guid.Parse(args.DeviceId), args.Alias);
        var homeDevice = homeDeviceService.Create(hdArgs);
        homeService.AssociateDevice(homeId, homeDevice.HardwareId);
        return Ok("Device added successfully to the home");
    }

    [HttpGet]
    [Route("{homeId}/members")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult ListMembersOfHome(Guid homeId)
    {
        return Ok(homeService.ListMembersOfHome(homeId));
    }

    [HttpPut("{homeId}/notifications")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult UpdateMemberNotifications([FromRoute] Guid homeId, [FromBody] UpdateMemberNotificationsRequest request)
    {
        homeService.UpdateMemberNotifications(homeId, request.ToArgs());
        return Ok("Modifications saved successfully");
    }

    [HttpPut]
    [Route("{homeId}/rooms")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult AddRoomToHome([FromRoute] Guid homeId, [FromBody] AddRoomToHomeRequest newRoom)
    {
        homeService.AddRoomToHome(homeId, newRoom.RoomName);
        return Ok("Room added successfully");
    }

    [HttpPut]
    [Route("{homeId}/alias")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult AddAliasToHome([FromRoute] Guid homeId, [FromBody] AddAliasToHomeRequest newAlias)
    {
        homeService.AddAliasToHome(homeId, newAlias.Alias);
        return Ok("Alias added successfully");
    }

    [HttpGet]
    [Route("{homeId}/rooms")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult ListRoomsOfHome([FromRoute] Guid homeId)
    {
        return Ok(homeService.GetAllRoomsOfHome(homeId));
    }
}
