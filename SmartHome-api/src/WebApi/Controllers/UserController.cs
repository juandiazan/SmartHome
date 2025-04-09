using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Queries;

namespace WebApi.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult GetAllUsers([FromQuery] GetUsersQuery query)
    {
        return Ok(userService.GetAll(query.ToArgs()));
    }
}
