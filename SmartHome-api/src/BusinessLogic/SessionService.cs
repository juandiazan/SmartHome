using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public sealed class SessionService(ISessionRepository sessionRepository, IUserService userService) : ISessionService
{
    public string Login(string email, string password)
    {
        if (!userService.Exists(u => u.Email == email))
        {
            throw new KeyNotFoundException("User does not exist");
        }

        if (IsPasswordIncorrect(email, password))
        {
            throw new ArgumentException("Invalid password");
        }

        if (HasActiveSessionByEmail(email))
        {
            return sessionRepository.GetSessionTokenByEmail(email);
        }

        var newSession = new Session
        {
            Id = Guid.NewGuid(),
            SessionToken = Guid.NewGuid().ToString(),
            User = userService.GetUserByEmail(email)
        };

        sessionRepository.Add(newSession);

        return newSession.SessionToken;
    }

    public bool HasActiveSessionByEmail(string email)
    {
        return sessionRepository.HasActiveSessionByEmail(email);
    }

    public bool IsAuthenticated(string token)
    {
        return sessionRepository.IsAuthenticated(token);
    }

    public User GetUserByToken(string token)
    {
        var loggedUser = sessionRepository.GetUserByToken(token)
            ?? throw new KeyNotFoundException("User is not in session");

        return loggedUser;
    }

    private bool IsPasswordIncorrect(string email, string password)
    {
        return !userService.IsPasswordCorrect(email, password);
    }
}
