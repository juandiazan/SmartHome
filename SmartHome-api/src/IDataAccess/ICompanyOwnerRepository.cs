using Domain;

namespace IDataAccess;
public interface ICompanyOwnerRepository : IUserGeneralRepository<CompanyOwner>
{
    Guid GetCompanyOwnerRoleId();
    void AssociateCompany(Guid company, Guid companyOwner);
    CompanyOwner GetById(Guid userId);
    CompanyOwner Update(CompanyOwner userToBeUpdated);
    Guid GetCompanyOwnerHomeOwnerRoleId();
}
