using System.Linq.Expressions;
using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;
public class HomeOwnerRepository : IHomeOwnerRepository
{
    private readonly SmartHomeDBContext _context;
    private readonly DbSet<HomeOwner> _homeOwners;

    public HomeOwnerRepository(SmartHomeDBContext context)
    {
        _context = context;
        _homeOwners = context.Set<HomeOwner>();
    }

    public HomeOwner Add(HomeOwner homeOwner)
    {
        _homeOwners.Add(homeOwner);
        _context.SaveChanges();

        return homeOwner;
    }

    public Guid GetHomeOwnerRoleId()
    {
        return SmartHomeDBContext.HomeOwnerRoleId;
    }

    public Role GetRoleById(Guid roleId)
    {
        return _context.Roles.FirstOrDefault(r => r.Id == roleId)!;
    }

    public bool Exists(Expression<Func<HomeOwner, bool>> predicate)
    {
        return _homeOwners.Any(predicate);
    }
}
