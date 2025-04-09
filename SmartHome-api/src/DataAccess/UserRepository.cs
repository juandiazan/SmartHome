using System.Linq.Expressions;
using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace DataAccess;

public class UserRepository(SmartHomeDBContext context) : IAdministratorRepository
{
    private readonly DbSet<User> _users = context.Users;

    public User Add(User user)
    {
        _users.Add(user);
        context.SaveChanges();

        return user;
    }

    public List<User> GetAll(UserFilterArgs args)
    {
        var users = _users.Include(u => u.Role);

        return PaginationFilterService.FilterAndPaginateUsers(users, args);
    }

    public User Update(User userToBeUpdated)
    {
        context.Update(userToBeUpdated);
        context.SaveChanges();

        return userToBeUpdated;
    }

    public User DeleteById(Guid userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId)!;

        _users.Remove(user);
        context.SaveChanges();

        return user;
    }

    public bool Exists(Expression<Func<User, bool>> predicate)
    {
        return _users.Any(predicate);
    }

    public Role GetRoleById(Guid roleId)
    {
        return context.Roles.FirstOrDefault(r => r.Id == roleId)!;
    }

    public User GetUserById(Guid userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId)!;
    }

    public User GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email)!;
    }

    public bool IsPasswordCorrect(string email, string password)
    {
        return _users.Any(u => u.Email == email && u.Password == password);
    }

    public Guid GetAdminRoleId()
    {
        return SmartHomeDBContext.AdministratorRoleId;
    }

    public Guid GetAdminHomeOwnerRoleId()
    {
        return SmartHomeDBContext.AdministratorHomeOwnerRoleId;
    }
}
