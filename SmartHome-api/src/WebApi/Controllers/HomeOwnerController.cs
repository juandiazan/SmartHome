using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("/home-owners")]
public class HomeOwnerController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateHomeOwner([FromBody] CreateHomeOwnerRequest homeOwnerArgs)
    {
        var args = homeOwnerArgs.ToArgs();
        var newHomeOwner = homeOwnerService.Create(args);
        return Created(
            $"home-owners/{newHomeOwner.Id}",
            new
            {
                message = "Account created successfully",
                CreatedHomeOwner = new CreateHomeOwnerResponse(
                newHomeOwner.Id.ToString(),
                newHomeOwner.Name,
                newHomeOwner.Surname,
                newHomeOwner.Email,
                newHomeOwner.Password,
                newHomeOwner.CreationDate.ToString(),
                newHomeOwner.Role.RoleName,
                newHomeOwner.ProfilePicture)
            });
    }

    [HttpGet]
    [Route("ownedHomeId")]
    [CustomAuthorizeFilter("home-owner", "admin-home-owner", "company-owner-home-owner")]
    public IActionResult GetHomeOwnerOwnedHomeId([FromHeader] string authorization)
    {
        return Ok(homeOwnerService.GetHomeOwnerOwnedHomeId(authorization));
    }
}
