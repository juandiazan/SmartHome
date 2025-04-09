using IBusinessLogic;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("/sessions")]
public class AuthenticationController(ISessionService sessionService) : ControllerBase
{
    [HttpPost]
    public IActionResult LogIn(LoginRequest request)
    {
        var token = sessionService.Login(request.Email, request.Password);
        return Ok(token);
    }

    [HttpGet("user-role")]
    public IActionResult GetRoleOfLoggedUser([FromHeader] string authorization)
    {
        return Ok(sessionService.GetUserByToken(authorization).Role.RoleName);
    }
}
