using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Common;

public class CustomPermissionFilter(string requiredPermission) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.ToString();
        var sessionService = context.HttpContext.RequestServices.GetService<ISessionService>();
        var homeService = context.HttpContext.RequestServices.GetService<IHomeService>();

        if (string.IsNullOrEmpty(token) || !sessionService!.IsAuthenticated(token))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Unauthorized: Authentication token is missing or invalid."
            };
            return;
        }

        var user = sessionService.GetUserByToken(token);

        var homeId = context.RouteData.Values["homeId"]?.ToString();

        if (string.IsNullOrEmpty(homeId))
        {
            var hardwareId = context.RouteData.Values["hardwareId"]?.ToString();
            homeId = homeService!.GetHomeIdByHardwareId(Guid.Parse(hardwareId!)).ToString();
        }

        if (string.IsNullOrEmpty(homeId) || !Guid.TryParse(homeId, out var homeGuid))
        {
            context.Result = new ContentResult
            {
                StatusCode = 400,
                Content = "Bad Request: Home ID is missing or invalid."
            };
            return;
        }

        var homeMembers = homeService.GetHomeMembers(homeGuid);
        var member = homeMembers.FirstOrDefault(m => m.AssociatedHomeOwnerId == user.Id);

        if (member == null || !member.Permissions.Any(p => p.Name == requiredPermission))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Forbidden: User does not have the required permissions."
            };
            return;
        }
    }
}
