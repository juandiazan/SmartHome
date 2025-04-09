using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaginationAndFilters.Models;
using WebApi.Controllers;
using WebApi.Models.Queries;

namespace WebApi.Test;
[TestClass]
public class NotificationControllerTest
{
    [TestMethod]
    public void GetAllNotifications_WithValidMemberId_ShouldReturnNotifications()
    {
        var notificationService = new Mock<INotificationService>(MockBehavior.Strict);

        notificationService
            .Setup(service => service.GetUserNotifications("token", new NotificationFilterArgs()))
            .Returns([]);

        var notificationController = new NotificationController(notificationService.Object);

        var result = notificationController.GetUserNotifications(new GetNotificationsQuery(), "token");

        result.Should().BeOfType<OkObjectResult>();
    }
}
