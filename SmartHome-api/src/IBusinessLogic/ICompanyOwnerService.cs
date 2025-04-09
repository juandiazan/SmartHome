using Domain;
using DTOs;

namespace IBusinessLogic;
public interface ICompanyOwnerService
{
    CompanyOwner Create(CreateUserArgs args);
    CompanyOwner GiveHomeOwnerRoleToCompanyOwner(Guid userToBeUpdated);
    CompanyOwner GetById(Guid userId);
}
