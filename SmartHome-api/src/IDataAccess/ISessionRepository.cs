using Domain;

namespace IDataAccess;
public interface ISessionRepository
{
    void Add(Session newSession);
    bool HasActiveSessionByEmail(string email);
    bool IsAuthenticated(string token);
    User? GetUserByToken(string token);

    string GetSessionTokenByEmail(string email);
}
