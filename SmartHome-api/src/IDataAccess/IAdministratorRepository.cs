using Domain;
using PaginationAndFilters.Models;

namespace IDataAccess;
public interface IAdministratorRepository : IUserGeneralRepository<User>
{
    List<User> GetAll(UserFilterArgs args);
    User Update(User userToBeUpdated);
    User DeleteById(Guid userId);
    User GetUserById(Guid userId);
    User GetUserByEmail(string email);
    bool IsPasswordCorrect(string email, string password);
    Guid GetAdminRoleId();
    Guid GetAdminHomeOwnerRoleId();
}
