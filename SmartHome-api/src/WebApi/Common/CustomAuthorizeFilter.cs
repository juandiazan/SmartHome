using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Common;

public class CustomAuthorizeFilter(params string[] roles) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context != null)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            var sessionService = context.HttpContext.RequestServices.GetService<ISessionService>();
            if (!string.IsNullOrEmpty(token) && sessionService!.IsAuthenticated(token))
            {
                if (roles == null || roles.Length == 0)
                {
                    return;
                }

                var user = sessionService.GetUserByToken(token);
                if (!roles.Contains(user.Role.RoleName))
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = 403,
                        Content = "Forbidden: User does not have the required permissions."
                    };
                }
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Unauthorized: Authentication token is missing or invalid."
                };
            }
        }
        else
        {
            context!.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Unauthorized: Authorization context is null."
            };
        }
    }
}
