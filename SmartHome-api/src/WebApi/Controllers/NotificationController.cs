using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Queries;

namespace WebApi.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    [CustomAuthorizeFilter]
    public IActionResult GetUserNotifications([FromQuery] GetNotificationsQuery query, [FromHeader] string? authorization)
    {
        var notifications = notificationService.GetUserNotifications(authorization, query.ToArgs());
        return Ok(notifications);
    }
}
