using Domain;

namespace IBusinessLogic;

public interface ISessionService
{
    public User GetUserByToken(string token);
    bool IsAuthenticated(string token);
    string Login(string email, string password);
}
