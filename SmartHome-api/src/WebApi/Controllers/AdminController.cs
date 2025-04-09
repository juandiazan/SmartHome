using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("administrators")]
public sealed class AdminController(IUserService userService, ISessionService sessionService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult CreateAdministrator([FromBody] CreateUserRequest request)
    {
        var newAdministrator = userService.Create(request.ToArgs());
        return Created(
            $"administrators/{newAdministrator.Id}",
            new
            {
                message = "Administrator account created successfully",
                CreatedAdministrator = new CreateUserResponse(
                newAdministrator.Id.ToString(),
                newAdministrator.Name,
                newAdministrator.Surname,
                newAdministrator.Email,
                newAdministrator.Password,
                newAdministrator.CreationDate.ToString(),
                newAdministrator.Role.RoleName)
            });
    }

    [HttpDelete("{id}")]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult DeleteAdministrator(Guid id)
    {
        userService.DeleteById(id);
        return Ok("Account deleted successfully");
    }

    [HttpPut("register-as-home-owner")]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult GiveHomeOwnerPermissionsToAdmin([FromHeader] string authorization)
    {
        var user = sessionService.GetUserByToken(authorization);
        userService.UpdateRoleOfAdministratorToHomeOwner(user.Id);
        return Ok("Permissions granted successfully");
    }
}
