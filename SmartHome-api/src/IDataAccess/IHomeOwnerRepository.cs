using Domain;

namespace IDataAccess;
public interface IHomeOwnerRepository : IUserGeneralRepository<HomeOwner>
{
    Guid GetHomeOwnerRoleId();
}
