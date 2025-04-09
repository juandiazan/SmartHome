using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaginationAndFilters.Models;
using WebApi.Controllers;
using WebApi.Models.Queries;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class CompanyControllerTest
{
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreate()
    {
        var token = "token";
        var modelValidatorId = Guid.NewGuid().ToString();

        var request = new CreateCompanyRequest(
            "Name",
            "Logo",
            "Rut",
            modelValidatorId);

        var newUser = new CompanyOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "email@gmail.com",
            Password = "pasWord123!"
        };

        var expectedCompany = new Company
        {
            Id = Guid.NewGuid(),
            Rut = request.Rut!,
            CompanyName = request.CompanyName!,
            Logotype = request.Logotype!,
            DeviceModelValidatorId = Guid.Parse(request.ModelValidatorId!)
        };

        var companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        var companyOwnerService = new Mock<ICompanyOwnerService>(MockBehavior.Strict);

        companyService
            .Setup(act => act.Create(request.ToArgs(), token))
            .Returns(expectedCompany);

        var companyController = new CompanyController(companyService.Object);

        var result = companyController.CreateCompany(request, token);

        result.Should().BeOfType<CreatedResult>();
    }

    [TestMethod]
    [DataRow(null, "Logo", "Rut", "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("", "Logo", "Rut", "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("Name", null, "Rut", "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("Name", "", "Rut", "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("Name", "Logo", null, "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("Name", "Logo", "", "37ccb7cc-31ba-454f-8c5d-28cb6e600241")]
    [DataRow("Name", "Logo", "Rut", null)]
    [DataRow("Name", "Logo", "Rut", "")]
    public void Create_WithInvalidArgument_ShouldBadRequest(string name, string logo, string rut, string modelValidatorId)
    {
        var token = "token";

        var request = new CreateCompanyRequest(
            name,
            logo,
            rut,
            modelValidatorId);

        var companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        var companyController = new CompanyController(companyService.Object);

        Action act = () => companyController.CreateCompany(request, token);
        act.Should().Throw<Exception>();
    }

    [TestMethod]
    public void GetAllCompanies_WithCorrectData_ShouldReturnOkResult()
    {
        // Arrange
        var companyServiceMock = new Mock<ICompanyService>(MockBehavior.Strict);
        var args = new CompanyFilterArgs(1, 10, "Company", "Juan");
        var query = new GetCompaniesQuery(1, 10, "Company", "Juan");

        var companies = new List<GetAllCompaniesCompanyArgs>
        {
            new("Name", "Logo", "Rut", "12930123"),
            new("Name", "Logo", "Rut", "12930123"),
        };

        companyServiceMock.Setup(service => service.GetAllCompanies(args))
            .Returns(companies);

        var companyController = new CompanyController(companyServiceMock.Object);

        // Act
        var result = companyController.GetAllCompanies(query);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(companies);
    }

    [TestMethod]
    public void GetAllCompanies_WithNegativeOffset_ShouldThrowFormatException()
    {
        // Arrange
        var companyServiceMock = new Mock<ICompanyService>(MockBehavior.Strict);
        var args = new CompanyFilterArgs(-1, 10, "Company", "Juan");
        var query = new GetCompaniesQuery(-1, 10, "Company", "Juan");

        companyServiceMock.Setup(service => service.GetAllCompanies(args))
            .Throws<FormatException>();

        var companyController = new CompanyController(companyServiceMock.Object);

        // Act
        Action act = () => companyController.GetAllCompanies(query);

        // Assert
        act.Should().Throw<FormatException>();
    }
}
