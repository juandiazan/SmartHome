using Domain;
using PaginationAndFilters.Models;

namespace IDataAccess;
public interface ICompanyRepository : IAddExistsRepository<Company>
{
    List<Company> GetAll(CompanyFilterArgs args);

    Company? GetCompanyByOwnerId(Guid ownerId);
}
