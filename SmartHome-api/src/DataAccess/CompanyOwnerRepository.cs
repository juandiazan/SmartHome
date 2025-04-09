using System.Linq.Expressions;
using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;
public class CompanyOwnerRepository : ICompanyOwnerRepository
{
    private readonly SmartHomeDBContext _context;
    private readonly DbSet<CompanyOwner> _companyOwners;

    public CompanyOwnerRepository(SmartHomeDBContext context)
    {
        _context = context;
        _companyOwners = context.Set<CompanyOwner>();
    }

    public CompanyOwner Add(CompanyOwner companyOwner)
    {
        _companyOwners.Add(companyOwner);
        _context.SaveChanges();

        return companyOwner;
    }

    public void AssociateCompany(Guid company, Guid companyOwner)
    {
        var companyOwnerToAssociate = _context.CompanyOwners.FirstOrDefault(co => co.Id == companyOwner)!;

        companyOwnerToAssociate.AssociatedCompanyId = company;
        companyOwnerToAssociate.AccountState = true;

        _context.Update(companyOwnerToAssociate);

        _context.SaveChanges();
    }

    public Guid GetCompanyOwnerRoleId()
    {
        return SmartHomeDBContext.CompanyOwnerRoleId;
    }

    public Role GetRoleById(Guid roleId)
    {
        return _context.Roles.FirstOrDefault(r => r.Id == roleId)!;
    }

    public bool Exists(Expression<Func<CompanyOwner, bool>> predicate)
    {
        return _companyOwners.Any(predicate);
    }

    public CompanyOwner GetById(Guid userId)
    {
        return _companyOwners.Include(co => co.AssociatedCompany).FirstOrDefault(u => u.Id == userId)!;
    }

    public CompanyOwner Update(CompanyOwner userToBeUpdated)
    {
        _context.Update(userToBeUpdated);
        _context.SaveChanges();

        return userToBeUpdated;
    }

    public Guid GetCompanyOwnerHomeOwnerRoleId()
    {
        return SmartHomeDBContext.CompanyOwnerHomeOwnerRoleId;
    }
}
