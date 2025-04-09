using System.Linq.Expressions;
using Domain;
using DTOs;
using PaginationAndFilters.Models;

namespace IBusinessLogic;

public interface IUserService
{
    User Create(CreateUserArgs args);
    List<GetAllUserArgs> GetAll(UserFilterArgs args);
    User DeleteById(Guid id);
    User GetUserByEmail(string email);
    bool IsPasswordCorrect(string email, string password);
    bool Exists(Expression<Func<User, bool>> predicate);
    User GetUserById(Guid userId);
    User UpdateRoleOfAdministratorToHomeOwner(Guid userToBeUpdated);
}
