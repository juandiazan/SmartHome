using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Queries;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("companies")]
public sealed class CompanyController(ICompanyService companyService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult CreateCompany([FromBody] CreateCompanyRequest request, [FromHeader] string authorization)
    {
        var newCompany = companyService.Create(request.ToArgs(), authorization);
        return Created(
            $"companies/{newCompany.Id}",
            new { message = "Company created successfully", CreatedCompany = new CreateCompanyResponse(newCompany.Id.ToString(), newCompany.CompanyName, newCompany.Logotype, newCompany.Rut) });
    }

    [HttpGet]
    [CustomAuthorizeFilter("administrator", "admin-home-owner")]
    public IActionResult GetAllCompanies([FromQuery] GetCompaniesQuery query)
    {
        return Ok(companyService.GetAllCompanies(query.ToArgs()));
    }
}
