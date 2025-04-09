using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Common;

namespace WebApi.Test;

[TestClass]
public class CustomPermissionFilterTest
{
    [TestMethod]
    public void OnAuthorization_UserIsAuthenticatedButNotEnoughPermission_ShouldSetForbiddenResult()
    {
        // Arrange
        var permission = "create-home";
        var filter = new CustomPermissionFilter(permission);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "valid-token";
        var homeId = Guid.NewGuid().ToString();
        httpContext.Request.RouteValues["homeId"] = homeId;

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            []);

        authFilterContext.RouteData.Values["homeId"] = homeId;

        var mockSessionService = new Mock<ISessionService>();
        var userId = Guid.NewGuid();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);
        mockSessionService.Setup(s => s.GetUserByToken(It.IsAny<string>())).Returns(new User { Id = userId });

        var mockHomeService = new Mock<IHomeService>();
        mockHomeService.Setup(h => h.GetHomeMembers(It.IsAny<Guid>())).Returns(
        [
            new Member
            {
                Id = userId,
                Permissions = [new Permission { Name = "some-other-permission" }]
            }

        ]);

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
            .AddTransient<IHomeService>(_ => mockHomeService.Object)
            .BuildServiceProvider();

        // Act
        filter.OnAuthorization(authFilterContext);

        // Assert
        Assert.IsInstanceOfType(authFilterContext.Result, typeof(ContentResult));
        var contentResult = authFilterContext.Result as ContentResult;
        Assert.AreEqual(403, contentResult.StatusCode);
        Assert.AreEqual("Forbidden: User does not have the required permissions.", contentResult.Content);
    }

    [TestMethod]
    public void OnAuthorization_UserIsNotAuthenticated_ShouldSetUnauthorizedResult()
    {
        // Arrange
        var filter = new CustomPermissionFilter("create-home");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "invalid-token";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            []);

        var mockSessionService = new Mock<ISessionService>();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(false);

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
            .BuildServiceProvider();

        // Act
        filter.OnAuthorization(authFilterContext);

        // Assert
        Assert.IsInstanceOfType(authFilterContext.Result, typeof(ContentResult));
        var contentResult = authFilterContext.Result as ContentResult;
        Assert.AreEqual(401, contentResult.StatusCode);
        Assert.AreEqual("Unauthorized: Authentication token is missing or invalid.", contentResult.Content);
    }

    [TestMethod]
    public void OnAuthorization_HomeIdIsInvalid_ShouldSetBadRequestResult()
    {
        // Arrange
        var filter = new CustomPermissionFilter("create-home");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "valid-token";
        httpContext.Request.RouteValues["homeId"] = "invalid-home-id";

        var routeData = new RouteData();
        routeData.Values["homeId"] = "invalid-home-id";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = new ActionDescriptor()
            },
            []);

        var mockSessionService = new Mock<ISessionService>();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
            .BuildServiceProvider();

        // Act
        filter.OnAuthorization(authFilterContext);

        // Assert
        Assert.IsInstanceOfType(authFilterContext.Result, typeof(ContentResult));
        var contentResult = authFilterContext.Result as ContentResult;
        Assert.AreEqual(400, contentResult.StatusCode);
        Assert.AreEqual("Bad Request: Home ID is missing or invalid.", contentResult.Content);
    }
}
