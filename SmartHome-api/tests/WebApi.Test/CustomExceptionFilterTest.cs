using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Common;

namespace WebApi.Test;

[TestClass]
public class CustomExceptionFilterTest
{
    private ExceptionContext _context = null!;
    private readonly CustomExceptionFilter _attribute;

    public CustomExceptionFilterTest()
    {
        _attribute = new CustomExceptionFilter(Mock.Of<ILogger<CustomExceptionFilter>>());
    }

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            []);
    }

    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new Exception("Not registered");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        GetTitle(concreteResponse.Value!).Should().Be("Not registered");
    }

    [TestMethod]
    public void OnException_WhenArgumentNullException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentNullException("param");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetTitle(concreteResponse.Value!).Should().Be("Value cannot be null. (Parameter 'param')");
    }

    [TestMethod]
    public void OnException_WhenArgumentException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentException("Invalid argument");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetTitle(concreteResponse.Value!).Should().Be("Invalid argument");
    }

    [TestMethod]
    public void OnException_WhenUnauthorizedAccessException_ShouldResponseUnauthorized()
    {
        _context.Exception = new UnauthorizedAccessException("Unauthorized");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetTitle(concreteResponse.Value!).Should().Be("Unauthorized");
    }

    [TestMethod]
    public void OnException_WhenKeyNotFoundException_ShouldResponseNotFound()
    {
        _context.Exception = new KeyNotFoundException("Not found");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        GetTitle(concreteResponse.Value!).Should().Be("Not found");
    }

    private string GetTitle(object value)
    {
        return value.GetType().GetProperty("Title").GetValue(value).ToString();
    }

    [TestMethod]
    public void OnException_WhenInvalidOperationException_ShouldResponseConflict()
    {
        _context.Exception = new InvalidOperationException("Invalid operation");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        GetTitle(concreteResponse.Value!).Should().Be("Invalid operation");
    }

    [TestMethod]
    public void OnException_WhenFormatException_ShouldResponseBadRequest()
    {
        _context.Exception = new FormatException("Invalid format");

        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Value.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetTitle(concreteResponse.Value!).Should().Be("Invalid format");
    }
}
