using System.Linq.Expressions;
using Domain;

namespace IDataAccess;
public interface IUserGeneralRepository<TUser>
{
    TUser Add(TUser newUser);
    bool Exists(Expression<Func<TUser, bool>> predicate);
    Role GetRoleById(Guid roleId);
}
