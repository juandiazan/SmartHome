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
public class CustomAuthorizeFilterTest
{
    [TestMethod]
    public void OnAuthorization_UserIsAuthenticatedAndAuthorized_ShouldNotSetResult()
    {
        // Arrange
        var role = "Admin";
        var filter = new CustomAuthorizeFilter(role);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "valid-token";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>());

        var mockSessionService = new Mock<ISessionService>();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);
        mockSessionService.Setup(s => s.GetUserByToken(It.IsAny<string>())).Returns(new User { Role = new Role { RoleName = role } });

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
            .BuildServiceProvider();

        // Act
        filter.OnAuthorization(authFilterContext);

        // Assert
        Assert.IsNull(authFilterContext.Result);
    }

    [TestMethod]
    public void OnAuthorization_UserIsAuthenticatedButNotAuthorized_ShouldSetForbiddenResult()
    {
        // Arrange
        var role = "Admin";
        var filter = new CustomAuthorizeFilter(role);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "valid-token";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>());

        var mockSessionService = new Mock<ISessionService>();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);
        mockSessionService.Setup(s => s.GetUserByToken(It.IsAny<string>())).Returns(new User { Role = new Role { RoleName = "User" } });

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
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
    public void OnAuthorization_UserIsAuthenticatedWithNoRole_CanDoEverything()
    {
        // Arrange
        var filter = new CustomAuthorizeFilter();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "valid-token";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>());

        var mockSessionService = new Mock<ISessionService>();
        mockSessionService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);

        httpContext.RequestServices = new ServiceCollection()
            .AddTransient<ISessionService>(_ => mockSessionService.Object)
            .BuildServiceProvider();

        // Act
        filter.OnAuthorization(authFilterContext);

        // Assert
        Assert.IsNull(authFilterContext.Result);
    }

    [TestMethod]
    public void OnAuthorization_UserIsNotAuthenticated_ShouldSetUnauthorizedResult()
    {
        // Arrange
        var filter = new CustomAuthorizeFilter();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "invalid-token";

        var authFilterContext = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>());

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
}
