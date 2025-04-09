using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("company-owners")]
public class CompanyOwnerController(ICompanyOwnerService userService, ISessionService sessionService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult CreateCompanyOwner([FromBody] CreateCompanyOwnerRequest request)
    {
        var args = request.ToArgs();
        var result = userService.Create(args);
        return Created(
            $"company-owners/{result.Id}",
            new
            {
                message = "Company Owner account created successfully",
                CreatedCompanyOwner = new CreateUserResponse(
                result.Id.ToString(),
                result.Name,
                result.Surname,
                result.Email,
                result.Password,
                result.CreationDate.ToString(),
                result.Role.RoleName)
            });
    }

    [HttpPut("register-as-home-owner")]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult GiveHomeOwnerPermissionsToCompanyOwner([FromHeader] string authorization)
    {
        var user = sessionService.GetUserByToken(authorization);
        userService.GiveHomeOwnerRoleToCompanyOwner(user.Id);
        return Ok("Permissions granted successfully");
    }
}
