using Domain;
using DTOs;
using PaginationAndFilters.Models;

namespace IBusinessLogic;
public interface ICompanyService
{
    Company Create(CreateCompanyArgs args, string sessionToken);
    List<GetAllCompaniesCompanyArgs> GetAllCompanies(CompanyFilterArgs args);
    Company GetCompanyByOwnerId(Guid ownerId);
}
