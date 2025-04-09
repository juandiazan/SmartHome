using Domain;
using DTOs;

namespace IBusinessLogic;

public interface IHomeOwnerService
{
    HomeOwner Create(CreateHomeOwnerArgs args);
    string GetHomeOwnerOwnedHomeId(string token);
}
