using System.Linq.Expressions;
using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace DataAccess;
public class CompanyRepository(SmartHomeDBContext context) : ICompanyRepository
{
    private readonly DbSet<Company> _companies = context.Set<Company>();

    public Company Add(Company newCompany)
    {
        _companies.Add(newCompany);

        context.SaveChanges();

        return newCompany;
    }

    public bool Exists(Expression<Func<Company, bool>> predicate)
    {
        return _companies.Any(predicate);
    }

    public List<Company> GetAll(CompanyFilterArgs args)
    {
        var companies = _companies.Include(x => x.CompanyOwner);

        return PaginationFilterService.FilterAndPaginateCompanies(companies, args);
    }

    public Company? GetCompanyByOwnerId(Guid ownerId)
    {
        return _companies.FirstOrDefault(c => c.CompanyOwner.Id == ownerId);
    }
}
